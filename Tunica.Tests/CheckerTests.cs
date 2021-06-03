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
        public void GitCredentialManagerCore()
        {
            using (var checker = new Checker())
            {
                var licenses = checker.Crawl(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "TestData", "Git-Credential-Manager-Core"));

                Assert.AreEqual(1, licenses.Count);
                Assert.AreEqual("MIT", licenses[0].Type);
            }
        }
    }
}
