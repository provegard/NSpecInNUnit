using NSpec;

namespace NSpecInNUnit
{
    // Run tests with something like DotCover (https://www.jetbrains.com/dotcover/)
    // Target.Sub shouldn't be covered, the other should be.
    public class cover : NUnitNSpec
    {
        public void describe_Target()
        {
            Target target = null;
            beforeAll = () => target = new Target();

            it["should add"] = () => target.Add(2, 3).should_be(5);
            it["should multiply"] = () => target.Mul(2, 3).should_be(6);
        }
    }

    public class Target
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Mul(int a, int b)
        {
            return a * b;
        }

        public int Sub(int a, int b)
        {
            return a - b;
        }
    }
}
