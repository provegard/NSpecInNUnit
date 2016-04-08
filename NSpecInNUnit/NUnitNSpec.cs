using System;
using System.Collections;
using System.Linq;
using System.Runtime.ExceptionServices;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace NSpecInNUnit
{
    public abstract class NUnitNSpec<T> : nspec
    {
        /// <summary>
        /// Test case source for the example runner test. This method generates a NUnit compatible
        /// test object for each discovered example. It's not a good idea to actually run the examples in
        /// this method, since it is called as part of NUnit's dicovery phase, which happens before setup
        /// fixtures are run.
        /// </summary>
        public static IEnumerable Examples()
        {
            var currentSpec = typeof (T);
            var finder = new SpecFinder(new[] { currentSpec });
            var builder = new ContextBuilder(finder, new Tags().Parse(currentSpec.Name), new DefaultConventions());
            try
            {
                var contextCollection = builder.Contexts().Build();
                var examples = contextCollection.SelectMany(ctx => ctx.AllExamples());
                return examples.Select(example => TestCaseDataFromExample(example, finder));
            }
            catch (Exception ex)
            {
                var example = new Example("Example discovery failure");
                example.Context = new Context("example discovery failure"); // for Example.FullName() to work
                example.Exception = ex;
                return new object[] { TestCaseDataFromExample(example, finder) };
            }
        }

        private static ITestCaseData TestCaseDataFromExample(ExampleBase example, SpecFinder finder)
        {
            var tcd = new TestCaseData(example, finder).SetName(example.FullName());
            if (example.Pending) tcd = tcd.Explicit();
            return tcd;
        }

        [TestCaseSource(nameof(Examples))]
        public void RunExample(ExampleBase example, SpecFinder finder)
        {
            var coll = new ContextCollection(new[] { example.Context });

            // Unsuccessful attempt to run one example per test:
            // var aTag = Guid.NewGuid().ToString();
            // example.Tags.Add(aTag);
            // ...and then `new Tags().Parse(aTag)` below.

            // A dummy ContextBuilder to get Tags into the ContextRunner
            var dummyBuilder = new ContextBuilder(finder, new Tags(), new DefaultConventions());
            var runner = new ContextRunner(dummyBuilder, new NoopFormatter(), false);

            var runCountBefore = example.Context.AllExamples().Count(_ => _.HasRun);
            runner.Run(coll);
            var runCountAfter = example.Context.AllExamples().Count(_ => _.HasRun);

            var examplesRun = runCountAfter - runCountBefore;
            if (examplesRun != 1) throw new Exception($"Expected 1 example to run, not {examplesRun}.");

            if (example.Failed())
            {
                if (example.Exception != null)
                {
                    // Use ExceptionDispatchInfo to preserve the original stack trace
                    var edi = ExceptionDispatchInfo.Capture(example.Exception);
                    edi.Throw();
                }
                throw new Exception("It failed :-(");
            }
        }
    }

    public class NoopFormatter : IFormatter
    {
        public void Write(ContextCollection contexts)
        {
        }
    }
}
