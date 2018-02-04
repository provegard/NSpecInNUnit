using System;
using System.Linq;
using FluentAssertions;
using NSpecInNUnit.Tests.Examples;
using NSpecInNUnit.Tests.Infra;
using NUnit.Framework;

namespace NSpecInNUnit.Tests.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class Example_realization
    {
        [Test]
        public void Realizes_a_single_example_in_a_class()
        {
            var collected = Runner.CollectOuputFrom<SingleExample>();
            collected.Should().Equal("example");
        }

        [Test]
        public void Realizes_multiple_examples_in_a_class()
        {
            var collected = Runner.CollectOuputFrom<MultipleExamples>();
            collected.Should().BeEquivalentTo("example1", "example2", "example3");
        }

        [Test]
        public void Runs_before_each()
        {
            var collected = Runner.CollectOuputFrom<SingleExampleWithBeforeEach>();
            collected.Should().Equal("before", "example");
        }

        [Test]
        public void Runs_before_each_before_each_example()
        {
            var collected = Runner.CollectOuputFrom<MultipleExamplesWithBeforeEach>();
            collected.Should().Equal("before", "example1", "before", "example2");
        }

        [Test]
        public void Runs_one_out_of_multiple_examples_with_before_each()
        {
            var collected = Runner.CollectOuputFrom<MultipleExamplesWithBeforeEach>("works again");
            collected.Should().Equal("before", "example2");
        }
        
        [Test]
        public void Runs_before_all_only_once()
        {
            var collected = Runner.CollectOuputFrom<MultipleExamplesWithBeforeAll>();
            collected.Where(c => c.Contains("beforeall")).Should().ContainSingle();
        }

        [Test]
        public void Runs_after_all_only_once()
        {
            var collected = Runner.CollectOuputFrom<MultipleExamplesWithAfterAll>();
            collected.Where(c => c.Contains("afterall")).Should().ContainSingle();
        }

        [Test]
        public void Runs_after_all_last()
        {
            var collected = Runner.CollectOuputFrom<MultipleExamplesWithAfterAll>().ToList();
            collected.Should().Equal("example1", "example2", "afterall");
        }
        
        [Test]
        public void Runs_after_all_in_block_only_once()
        {
            var collected = Runner.CollectOuputFrom<MultipleExamplesWithAfterAllInBlock>();
            collected.Where(c => c.Contains("afterall")).Should().ContainSingle();
        }

        [Test]
        public void Runs_async_after_all_only_once()
        {
            var collected = Runner.CollectOuputFrom<MultipleExamplesWithAsyncAfterAll>();
            collected.Where(c => c.Contains("afterall")).Should().ContainSingle();
        }
        
        [Test]
        public void Runs_async_after_all_in_block_only_once()
        {
            var collected = Runner.CollectOuputFrom<MultipleExamplesWithAsyncAfterAllInBlock>();
            collected.Where(c => c.Contains("afterall")).Should().ContainSingle();
        }
        
        [Test]
        public void Runs_before_each_on_multiple_levels()
        {
            var collected = Runner.CollectOuputFrom<SingleNestedExampleWithBeforeEachs>();
            collected.Should().Equal("outer_before", "inner_before", "example");
        }
        
        //TODO:
        // - nested beforeall/afterall
    }
}