using System.Threading.Tasks;
using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class MultipleExamplesWithAsyncAfterAllInBlock : nspec_as_nunit<MultipleExamplesWithAsyncAfterAllInBlock>
    {
        public void describe_stuff()
        {
            afterAllAsync = async () => Tracker.Log("afterall");
            it["works"] = () => Tracker.Log("example1");
            it["works again"] = () => Tracker.Log("example2");
        }
    }
}