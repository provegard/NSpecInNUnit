using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class SingleExampleWithBeforeEach : nspec_as_nunit<SingleExampleWithBeforeEach>
    {
        public void before_each()
        {
            Tracker.Log("before");
        }
        
        public void it_works()
        {
            Tracker.Log("example");
        }
    }
}