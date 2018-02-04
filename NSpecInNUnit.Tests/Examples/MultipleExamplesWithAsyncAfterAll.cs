using System.Threading.Tasks;
using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class MultipleExamplesWithAsyncAfterAll : nspec_as_nunit<MultipleExamplesWithAsyncAfterAll>
    {
        public async Task after_all()
        {
            await Task.Delay(1);
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