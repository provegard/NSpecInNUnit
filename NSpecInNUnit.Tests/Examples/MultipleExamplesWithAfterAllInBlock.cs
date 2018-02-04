using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class MultipleExamplesWithAfterAllInBlock : nspec_as_nunit<MultipleExamplesWithAfterAllInBlock>
    {
        public void describe_stuff()
        {
            afterAll = () => Tracker.Log("afterall");
            it["works"] = () => Tracker.Log("example1");
            it["works again"] = () => Tracker.Log("example2");
        }
    }
}