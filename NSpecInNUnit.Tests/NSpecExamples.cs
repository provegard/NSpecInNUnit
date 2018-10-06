using System.Collections.Generic;
using NSpec.Domain;

namespace NSpecInNUnit.Tests
{
    internal static class NSpecExamples
    {
        internal static IEnumerable<ExampleBase> For<T>()
            where T : nspec_as_nunit<T>
        {
            foreach (var obj in nspec_as_nunit<T>.Examples())
            {
                if (!(obj is NUnitTestFromExample data)) continue;
                var ctx = data.Arguments[0] as ExampleContext;
                yield return ctx.Example;
            }
        }
    }
}
