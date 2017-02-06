using NSpec;

namespace NSpecInNUnit
{
    public class a_spec : NUnitNSpec
    {
        public void describe_stuff()
        {
            var counter = 0;
            beforeEach = () => counter++;

            // Note that all examples are always run, even if one is singled out using ReSharper.
            it["should run in isolation"] = () => counter.should_be(1);
            it["should run in isolation also"] = () => counter.should_be(2);
            it["should run in isolation too"] = () => counter.should_be(3);
        }
    }
}
