using NSpecInNUnit.Tests.Infra;

namespace NSpecInNUnit.Tests.Examples
{
    public class MultipleExamples : nspec_as_nunit<MultipleExamples>
    {
        public void it_works()
        {
            Tracker.Log("example1");
        }
        public void it_works_again()
        {
            Tracker.Log("example2");
        }
        public void it_works_even_more()
        {
            Tracker.Log("example3");
        }
    }
}