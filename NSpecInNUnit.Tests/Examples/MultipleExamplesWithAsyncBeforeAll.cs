using System.Threading.Tasks;
using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class MultipleExamplesWithAsyncBeforeAll : nspec_as_nunit<MultipleExamplesWithAsyncBeforeAll>
    {
        public async Task before_all()
        {
            await Task.Delay(1);
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