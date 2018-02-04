using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class MultipleExamplesWithBeforeEach : nspec_as_nunit<MultipleExamplesWithBeforeEach>
    {
        public void before_each()
        {
            Tracker.Log("before");
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