using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NUnit.Framework;

namespace NSpecInNUnit
{
    /// <summary>
    /// Base class for NSpec-based tests. This class allows us to run tests in Visual Studio using
    /// ReSharper as well as from the console using the NUnit runner.
    /// </summary>
    public abstract class nspec_as_nunit<T> : nspec
        where T : nspec_as_nunit<T>
    {
        // Tracks the last executed context, for running after-all actions in OneTimeTearDown.
        // This member belongs to the NUnit-created instance of this class.
        private ExampleContext _lastContext;
        
        // List of after-all actions to run in OneTimeTearDown. The list belongs to the NSpec-created instance
        // of this class, not the NUnit-created instance.
        private IList<Action<nspec>> _afterAllActions;

        private void CollectAfterAll(Action<nspec> action)
        {
            if (_afterAllActions == null) _afterAllActions = new List<Action<nspec>>();
            _afterAllActions.Add(action);
        }
        
        public static IEnumerable Examples()
        {
            var currentSpec = typeof(T);
            var finder = new SpecFinder(new[] { currentSpec });
            var tagsFilter = new Tags();
            var builder = new ContextBuilder(finder, tagsFilter, new DefaultConventions());
            try
            {
                // The test case source is run during the discovery phase of NUnit. This means that we cannot
                // run the examples now - setup fixtures haven't run yet, for example. But we use the result
                // of the discovery phase to produce a number of test cases.
                var testSuite = builder.Contexts().Build();
                
                ContextCollectionUtil.PatchBeforeAllsToRunOnce(testSuite);
                var thisInstance = GetThisInstance(testSuite);
                ContextCollectionUtil.DeferAfterAlls(testSuite, a =>
                {
                    thisInstance.CollectAfterAll(a);
                });
                
                var examples = testSuite.SelectMany(ctx => ctx.AllExamples());
                return examples.Select(example =>
                {
                    // Add an example-specific ID so that we can run examples one by one later on.
                    var id = Guid.NewGuid();
                    example.Tags.AddRange(Tags.ParseTags(id.ToString()));
                    return new NUnitTestFromExample(tagsFilter, testSuite, example, id);
                });
            }
            catch (Exception ex)
            {
                // Generate a fake example that represents the setup failure.
                var context = new Context("test failure"); // for Example.FullName() to work
                var example = new Example("Test setup failure");
                example.Exception = ex;
                example.HasRun = true;
                example.AddTo(context);
                return new object[] { new NUnitTestFromExample(tagsFilter, null, example, Guid.Empty) };
            }
        }

        private static nspec_as_nunit<T> GetThisInstance(object instance)
        {
            if (!(instance is nspec_as_nunit<T> ret))
                throw new Exception($"Instance {instance} is not of type nspec_as_nunit<{typeof(T).Name}>");
            return ret;
        }

        private static nspec_as_nunit<T> GetThisInstance(ContextCollection testSuite)
        {
            var nspecInstance = testSuite.First().GetInstance(); //TODO fix ugly
            return GetThisInstance(nspecInstance);
        }

        [OneTimeTearDown]
        public void RunAfterAlls()
        {
            if (_lastContext?.TestSuite == null) return;

            var thisInstance = GetThisInstance(_lastContext.TestSuite);
            if (thisInstance._afterAllActions != null)
            {
                foreach (var action in thisInstance._afterAllActions)
                {
                    action(thisInstance);
                }
            }
        }
        
        [TestCaseSource(nameof(Examples))]
        public void RealizeSpec(ExampleContext ctx)
        {
            // Save for OneTimeTearDown
            _lastContext = ctx;

            File.AppendAllText("c:\\temp\\cont.txt", "RealizeSpec for " + ctx.Example.Spec + Environment.NewLine);

            //TODO: Instance lock here! How will that work with one instance per test??
            // TestSuite is null in the setup error case
            if (ctx.TestSuite != null)
            {
                var tagsFilter = ctx.TagsFilter;
                tagsFilter.IncludeTags.Add(ctx.TestId.ToString());
                try
                {
                    // Run the suite directly as opposed via ContextRunner since the latter trims contexts
                    // and examples which is undesirable given that we make multiple runs.
                    ctx.TestSuite.Run(new SilentLiveFormatter(), false);
                }
                finally
                {
                    tagsFilter.IncludeTags.Remove(ctx.TestId.ToString());
                }
            }

            var example = ctx.Example;

            WriteCapturedOutput(example);
            
            if (!example.HasRun) throw new Exception("The example didn't run :-(");
            if (example.Failed())
            {
                if (example.Exception != null)
                {
                    // Use ExceptionDispatchInfo to preserve the original stack trace
                    var edi = ExceptionDispatchInfo.Capture(example.Exception);
                    edi.Throw();
                }
                throw new Exception("The example failed, but I don't know why :-(");
            }
        }

        private void WriteCapturedOutput(ExampleBase example)
        {
            WriteCapturedOutput(example.Context);
            if (!string.IsNullOrEmpty(example.CapturedOutput))
            {
                // The output includes newlines, so use Console.Write here.
                Console.Write(example.CapturedOutput);
            }
        }

        private void WriteCapturedOutput(Context context)
        {
            var parent = context.Parent;
            if (parent != null) WriteCapturedOutput(parent);
            if (!string.IsNullOrEmpty(context.CapturedOutput))
            {
                Console.Write(context.CapturedOutput);
            }
        }
    }
}
