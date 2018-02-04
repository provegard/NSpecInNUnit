using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using NSpec;
using NSpec.Domain;
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
        private ExampleContext _lastContext;
        private readonly IList<Action<nspec>> _afterAllActions = new List<Action<nspec>>();

        private void CollectAfterAll(Action<nspec> action)
        {
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
                DeferAfterAlls(testSuite);
                var examples = testSuite.SelectMany(ctx => ctx.AllExamples());
                return examples.Select(example =>
                {
                    var id = Guid.NewGuid();
                    example.Tags.AddRange(Tags.ParseTags(id.ToString() + " __exclude"));
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

        private static void DeferAfterAlls(ContextCollection testSuite)
        {
            var rootContext = testSuite.SingleOrDefault();
            if (rootContext == null) throw new Exception("Failed to identify the root context");
            DeferAfterAll(rootContext);
        }

        private static void DeferAfterAll(Context context)
        {
            foreach (var subContext in context.Contexts) DeferAfterAll(subContext);
            var hook1 = context.AfterAll;
            if (hook1 != null)
            {
                context.AfterAll = () =>
                {
                    GetThisInstance(context.GetInstance()).CollectAfterAll(_ => hook1());
                };                
            }
            var hook2 = context.AfterAllInstance;
            if (hook2 != null)
            {
                context.AfterAllChain.SetProtectedProperty(_ => _.ClassHook, nspec =>
                {
                    GetThisInstance(nspec).CollectAfterAll(hook2);
                });
            }

            var hook3 = context.AfterAllInstanceAsync;
            if (hook3 != null)
            {
                context.AfterAllChain.SetProtectedProperty(_ => _.AsyncClassHook, nspec =>
                {
                    GetThisInstance(nspec).CollectAfterAll(hook3);
                });                
            }

            var hook4 = context.AfterAllAsync;
            if (hook4 != null)
            {
                context.AfterAllAsync = () =>
                {
                    GetThisInstance(context.GetInstance()).CollectAfterAll(_ => hook4().Wait());
                    return Task.Delay(0);
                };
            }
        }

        private static nspec_as_nunit<T> GetThisInstance(object instance)
        {
            if (!(instance is nspec_as_nunit<T> ret))
                throw new Exception($"Instance {instance} is not of type nspec_as_nunit<{typeof(T).Name}>");
            return ret;
        }

        [OneTimeTearDown]
        public void RunAfterAlls()
        {
            if (_lastContext?.TestSuite == null) return;
            var nspecInstance = _lastContext.TestSuite.First().GetInstance(); //TODO fix ugly
            var thisInstance = GetThisInstance(nspecInstance);
            foreach (var action in thisInstance._afterAllActions)
            {
                action(nspecInstance);
            }
        }

        [TestCaseSource(nameof(Examples))]
        public void RealizeSpec(ExampleContext ctx)
        {
            // Save for OneTimeTearDown
            _lastContext = ctx;

            //TODO: Instance lock here! How will that work with one instance per test??
            // TestSuite is null in the setup error case
            if (ctx.TestSuite != null)
            {
                var tagsFilter = ctx.TagsFilter;
                tagsFilter.IncludeTags.Add(ctx.TestId.ToString());
                try
                {
                    var runner = new ContextRunner(tagsFilter, new NoopFormatter(), false);
                    runner.Run(ctx.TestSuite);
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
