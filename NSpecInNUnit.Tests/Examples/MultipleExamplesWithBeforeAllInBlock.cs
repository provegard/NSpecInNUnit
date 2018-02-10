using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class MultipleExamplesWithBeforeAllInBlock : nspec_as_nunit<MultipleExamplesWithBeforeAllInBlock>
    {
        public void describe_stuff()
        {
            beforeAll = () => Tracker.Log("beforeall");
            it["works"] = () => Tracker.Log("example1");
            it["works again"] = () => Tracker.Log("example2");
        }
    }
}