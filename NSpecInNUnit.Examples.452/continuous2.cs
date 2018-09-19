using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NSpecInNUnit.Examples._452
{


    [TestFixture]
    class continuous2 
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void testit(int i)
        {
            switch (i)
            {
                case 1:
                    LogIt("add");
                    Assert.That(new Calc().Add(1, 2), Is.EqualTo(3));
                    break;
                case 2:
                    LogIt("sub");
                    Assert.That(new Calc().Sub(1, 2), Is.EqualTo(-1));
                    break;
                case 3:
                    LogIt("mul");
                    Assert.That(new Calc().Mul(1, 2), Is.EqualTo(2));
                    break;
            }
        }

        private void LogIt(string s)
        {
            File.AppendAllText("c:\\temp\\cont.txt", s + Environment.NewLine);
        }
    }
}
