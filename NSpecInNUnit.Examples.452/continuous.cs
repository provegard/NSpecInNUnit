using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NSpecInNUnit.Examples._452
{
    class Calc
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
        public int Sub(int a, int b)
        {
            return a - b;
        }
        public int Mul(int a, int b)
        {
            return a * b*1;
        }

    }

    class continuous : nspec_as_nunit<continuous>
    {
        public void describe_something()
        {
            it["tests add"] = () => {
                LogIt("add");
                Assert.That(new Calc().Add(1, 2), Is.EqualTo(3));
            };
            it["tests sub"] = () => {
                LogIt("sub");
                Assert.That(new Calc().Sub(1, 2), Is.EqualTo(-1));
            };
            it["tests mul"] = () => {
                LogIt("mul");
                Assert.That(new Calc().Mul(1, 2), Is.EqualTo(2));
            };
        }

        private void LogIt(string s)
        {
            File.AppendAllText("c:\\temp\\cont.txt", s + Environment.NewLine);
        }
    }
}
