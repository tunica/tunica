using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tunica.Tests
{
    [TestClass]
    public class CheckerTests
    {
        [TestMethod]
        public void Empty()
        {
            using (var checker = new Checker())
            {
                var licenses = checker.Crawl(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "TestData", "Empty"));

                Assert.AreEqual(0, licenses.Count);
            }
        }

        [TestMethod]
        public void Apache()
        {
            using (var checker = new Checker())
            {
                var licenses = checker.Crawl(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "TestData", "Apache"));

                Assert.AreEqual(1, licenses.Count);
                Assert.AreEqual("Apache-2.0", licenses[0].Type);
            }
        }

        [TestMethod]
        public void Mit()
        {
            using (var checker = new Checker())
            {
                var licenses = checker.Crawl(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "TestData", "MIT"));

                Assert.AreEqual(1, licenses.Count);
                Assert.AreEqual("MIT", licenses[0].Type);
            }
        }

        [TestMethod]
        public void Multiple()
        {
            using (var checker = new Checker())
            {
                var licenses = checker.Crawl(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "TestData"));

                Assert.AreEqual(2, licenses.Count);
                Assert.AreEqual("Apache-2.0", licenses[0].Type);
                Assert.AreEqual("MIT", licenses[1].Type);
            }
        }
    }
}
