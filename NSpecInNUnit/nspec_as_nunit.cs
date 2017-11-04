using System;
using System.Collections;
using System.Collections.Generic;
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
        private bool _hasRun;

        public static IEnumerable Examples()
        {
            var currentSpec = typeof (T);
            var finder = new SpecFinder(new[] { currentSpec });
            var tagsFilter = new Tags().Parse(currentSpec.Name);
            var builder = new ContextBuilder(finder, tagsFilter, new DefaultConventions());
            try
            {
                // The test case source is run during the discovery phase of NUnit. This means that we cannot
                // run the examples now - setup fixtures haven't run yet, for example. But we use the result
                // of the discovery phase to produce a number of test cases.
                var testSuite = builder.Contexts().Build();
                var examples = testSuite.SelectMany(ctx => ctx.AllExamples());
                return examples.Select(example => new NUnitTestFromExample(tagsFilter, testSuite, example));
            }
            catch (Exception ex)
            {
                // Generate a fake example that represents the setup failure.
                var example = new Example("Test setup failure");
                example.AddTo(new Context("test failure")); // for Example.FullName() to work
                example.Exception = ex;
                example.HasRun = true;
                return new object[] { new NUnitTestFromExample(tagsFilter, null, example) };
            }
        }

        [TestCaseSource(nameof(Examples))]
        public void RealizeSpec(ExampleContext ctx)
        {
            // If we haven't run the examples yet, do so now. TestSuite is null in case of setup failure. All
            // my attempts to run examples individually have failed (e.g. due to nested contexts or the use of
            // beforeAll), so run everything and use the individual test cases only for reporting.
            if (!_hasRun && ctx.TestSuite != null)
            {
                _hasRun = true;
                var runner = new ContextRunner(ctx.TagsFilter, new NoopFormatter(), false);
                runner.Run(ctx.TestSuite);
            }

            var example = ctx.Example;
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
    }

    /// <summary>
    /// An NSpec formatter that doesn't do anything.
    /// </summary>
    public class NoopFormatter : IFormatter
    {
        public void Write(ContextCollection contexts)
        {
        }

        public IDictionary<string, string> Options { get; set; }
    }

    /// <summary>
    /// An NUnit wrapper for NSpec information that at the same time acts as a single test case from a
    /// test case source.
    /// </summary>
    public class NUnitTestFromExample : TestCaseData
    {
        public NUnitTestFromExample(Tags tagsFilter, ContextCollection testSuite, ExampleBase example)
            : base(new ExampleContext(tagsFilter, testSuite, example))
        {
            if (example.Pending) Ignore("Ignored");
            SetName(example.FullName());
        }
    }

    /// <summary>
    /// Contains an NSpec example instance toghether with contextual information (instances) necessary
    /// to run the example.
    /// </summary>
    public class ExampleContext
    {
        public Tags TagsFilter { get; }
        public ContextCollection TestSuite { get; }
        public ExampleBase Example { get; }

        public ExampleContext(Tags tagsFilter, ContextCollection testSuite, ExampleBase example)
        {
            TagsFilter = tagsFilter;
            TestSuite = testSuite;
            Example = example;
        }
    }
}
