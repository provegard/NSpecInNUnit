using NSpec;

namespace NSpecInNUnit
{
    public class a_spec : NUnitNSpec<a_spec>
    {
        public void describe_stuff()
        {
            it["should run in isolation"] = () => 1.should_be(1);
            it["should run in isolation also"] = () => 1.should_be(1);
            it["should run in isolation too"] = () => 1.should_be(1);
        }
    }
}
