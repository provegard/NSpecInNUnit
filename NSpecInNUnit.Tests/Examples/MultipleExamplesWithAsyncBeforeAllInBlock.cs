using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class MultipleExamplesWithAsyncBeforeAllInBlock : nspec_as_nunit<MultipleExamplesWithAsyncBeforeAllInBlock>
    {
        public void describe_stuff()
        {
            beforeAllAsync = async () => Tracker.Log("beforeall");
            it["works"] = () => Tracker.Log("example1");
            it["works again"] = () => Tracker.Log("example2");
        }
    }
}