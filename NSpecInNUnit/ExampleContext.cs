using System;
using NSpec.Domain;

namespace NSpecInNUnit
{
    /// <summary>
    /// Contains an NSpec example instance toghether with contextual information (instances) necessary
    /// to run the example.
    /// </summary>
    public class ExampleContext
    {
        public Tags TagsFilter { get; }
        public ContextCollection TestSuite { get; }
        public ExampleBase Example { get; }
        public Guid TestId { get; } //TODO: String instead

        public ExampleContext(Tags tagsFilter, ContextCollection testSuite, ExampleBase example, Guid TestId)
        {
            TagsFilter = tagsFilter;
            TestSuite = testSuite;
            Example = example;
            this.TestId = TestId;
        }
    }
}