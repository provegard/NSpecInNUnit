using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class ExamplesInMultipleContexts : nspec_as_nunit<ExamplesInMultipleContexts>
    {
        public void describe_stuff()
        {
            context["main"] = () => { it["is run"] = () => Tracker.Log("example1"); };
            context["second"] = () => { it["is run"] = () => Tracker.Log("example2"); };
        }
        public void describe_other()
        {
            context["third"] = () => { it["is run"] = () => Tracker.Log("example3"); };
        }

    }
}