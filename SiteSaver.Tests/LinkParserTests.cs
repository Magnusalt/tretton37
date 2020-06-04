using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SiteSaver.Tests
{
    [TestClass]
    public class LinkParserTests
    {
        private ILinkParser _sut;

        [TestInitialize]
        public void Setup()
        {
            _sut = new HtmlLinkParser("altyard.solutions");
        }

        [TestMethod]
        public void FindLinks_ReturnsOneMatch_WhenDocumentContainsOneAElement()
        {
            var html = "<a href=\"altyard.solutions\">Altyard solutions AB</a>";

            var results = _sut.FindLinks(html, "somePath");

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("altyard.solutions", results.First());
        }

        [TestMethod]
        public void FindLinks_ReturnsCorrectPath_WhenLinkIsRelative()
        {
            var html = "<a href=\"../blog\">Blog</a>";

            var results = _sut.FindLinks(html, "main/about");
            
            Assert.AreEqual("blog", results.First());
        }
    }
}
