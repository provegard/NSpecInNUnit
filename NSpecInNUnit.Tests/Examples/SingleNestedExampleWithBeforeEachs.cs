using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class SingleNestedExampleWithBeforeEachs : nspec_as_nunit<SingleNestedExampleWithBeforeEachs>
    {
        public void before_each()
        {
            Tracker.Log("outer_before");
        }

        public void describe_stuff()
        {
            beforeEach = () => Tracker.Log("inner_before");
            it["works"] = () => Tracker.Log("example");
        }
    }
}