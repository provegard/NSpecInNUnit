using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class SingleExample : nspec_as_nunit<SingleExample>
    {
        public void it_works()
        {
            Tracker.Log("example");
        }
    }
}