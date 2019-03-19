using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Reporting
{
    [TestFixture, Parallelizable(ParallelScope.Fixtures)]
    public class TestClass : ReportGenerator
    {
        [Test]
        [CategoryAttribute("Sample Tests")]
        [CategoryAttribute("priority=4")]
        [Author("johndoe")]
        public void TestMethod1()
        {
            Assert.IsTrue(true);
        }

        [Test]
        [CategoryAttribute("Sample Tests")]
        [CategoryAttribute("priority=4")]
        [Author("johndoe")]
        public void TestMethod2()
        {
            Assert.IsTrue(true);
        }

        [Test]
        [CategoryAttribute("Sample Tests")]
        [CategoryAttribute("priority=4")]
        [Author("johndoe")]
        public void TestMethod3()
        {
            Assert.IsTrue(true);
        }

        [Test]
        [CategoryAttribute("Sample Tests")]
        [CategoryAttribute("priority=4")]
        [Author("johndoe")]
        public void TestMethod4()
        {
            Assert.IsTrue(true);
        }

        [Test]
        [CategoryAttribute("Sample Tests")]
        [CategoryAttribute("priority=4")]
        [Author("johndoe")]
        public void TestMethod5()
        {
            Assert.IsTrue(true);
        }

        [Test]
        [CategoryAttribute("Sample Tests")]
        [CategoryAttribute("priority=4")]
        [Author("johndoe")]
        public void TestMethod6()
        {
            Assert.IsTrue(true);
        }
    }
}
