using NUnit.Framework;

namespace NSpecInNUnit
{
    public class a_spec : nspec_as_nunit<a_spec>
    {
        public void describe_stuff()
        {
            it["should run in a .NET 4.5.2 project"] = () => Assert.That(1, Is.EqualTo(1));
        }
    }
}
