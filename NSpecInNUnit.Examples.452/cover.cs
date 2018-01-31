using NUnit.Framework;

namespace NSpecInNUnit.Examples._452
{
    // Run tests with something like DotCover (https://www.jetbrains.com/dotcover/)
    // Target.Sub shouldn't be covered, the other should be.
    public class cover : nspec_as_nunit<cover>
    {
        public void describe_Target()
        {
            Target target = null;
            beforeAll = () => target = new Target();

            it["should add"] = () => Assert.That(target.Add(2, 3), Is.EqualTo(5));
            it["should multiply"] = () => Assert.That(target.Mul(2, 3), Is.EqualTo(6));
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
