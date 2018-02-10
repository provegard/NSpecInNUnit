using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class MultipleExamplesWithNestedAfterAll : nspec_as_nunit<MultipleExamplesWithNestedAfterAll>
    {
        public void describe_stuff()
        {
            afterAll = () => Tracker.Log("afterall_inner");
            it["works"] = () => Tracker.Log("example1");
            it["works again"] = () => Tracker.Log("example2");
        }

        public void after_all()
        {
            Tracker.Log("afterall_outer");
        }
    }
}