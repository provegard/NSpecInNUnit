using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class MultipleExamplesWithAfterAll : nspec_as_nunit<MultipleExamplesWithAfterAll>
    {
        public void after_all()
        {
            Tracker.Log("afterall");
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