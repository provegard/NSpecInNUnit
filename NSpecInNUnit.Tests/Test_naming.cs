using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NSpecInNUnit.Tests
{
    public class Test_naming : nspec_as_nunit<Test_naming>
    {
        public void it_handles_toplevel_it()
        {
            var example = NSpecExamples.For<Simple_it>().First();
            var name = TestNamer.CreateReSharperSafeName(example);
            Assert.That(name, Is.EqualTo("Simple it, works"));
        }

        public void it_handles_it_inside_describe()
        {
            var example = NSpecExamples.For<It_inside_describe>().First();
            var name = TestNamer.CreateReSharperSafeName(example);
            Assert.That(name, Is.EqualTo("It inside describe, stuff, works"));
        }

        public class Simple_it : nspec_as_nunit<Simple_it>
        {
            public void it_works()
            {
            }
        }

        public class It_inside_describe : nspec_as_nunit<It_inside_describe>
        {
            public void describe_stuff()
            {
                it["works"] = () => { };
            }
        }
    }
}
