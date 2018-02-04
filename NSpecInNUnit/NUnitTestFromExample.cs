using System;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecInNUnit
{
    /// <summary>
    /// An NUnit wrapper for NSpec information that at the same time acts as a single test case from a
    /// test case source.
    /// </summary>
    internal class NUnitTestFromExample : TestCaseData
    {
        public NUnitTestFromExample(Tags tagsFilter, ContextCollection testSuite, ExampleBase example, Guid testId)
            : base(new ExampleContext(tagsFilter, testSuite, example, testId))
        {
            if (example.Pending) Ignore("Ignored");
            SetName(example.FullName());
        }
    }
}