using NUnit.Framework;

namespace NSpecInNUnit
{
    public class a_spec : nspec_as_nunit<a_spec>
    {
        public void describe_stuff()
        {
            var counter = 0;
            beforeEach = () => counter++;

            // Note that all examples are always run, even if one is singled out using ReSharper.
            it["should run in isolation"] = () => Assert.That(counter, Is.EqualTo(1));
            it["should run in isolation also"] = () => Assert.That(counter, Is.EqualTo(2));
            it["should run in isolation too"] = () => Assert.That(counter, Is.EqualTo(3));
        }
    }
}
