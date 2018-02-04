using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class MultipleExamplesWithBeforeAll : nspec_as_nunit<MultipleExamplesWithBeforeAll>
    {
        public void before_all()
        {
            Tracker.Log("beforeall");
        }
        public void it_works()
        {
            Tracker.Log("example1");
        }
        public void it_works_again()
        {
            Tracker.Log("example2");
        }
    }
}