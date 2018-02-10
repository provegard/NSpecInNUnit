using System;
using System.Collections.Generic;
using System.Linq;
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
            SetName(TestName(example));
        }

        private static string TestName(ExampleBase example)
        {
            //TODO: Should we strip 'describe' and 'it' prefixes here?
            // Skip contexts nspec and test-class
            var contexts = ContextsOf(example).Skip(2);
            return string.Join(". ", contexts.Select(c => c.Name)) + ". " + example.Spec + ".";
        }

        private static IEnumerable<Context> ContextsOf(ExampleBase example)
        {
            foreach (var ctx in ContextsOf(example.Context)) yield return ctx;
            yield return example.Context;
        }
        
        private static IEnumerable<Context> ContextsOf(Context context)
        {
            var parent = context.Parent;
            if (parent != null)
            {
                foreach (var ctx in ContextsOf(parent)) yield return ctx;
                yield return parent;
            }
        }
    }
}