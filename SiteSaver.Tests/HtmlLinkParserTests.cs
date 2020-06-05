using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SiteSaver.Tests
{
    [TestClass]
    public class HtmlLinkParserTests
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
            var html = "<a href=\"some/path\">A link</a>";
            var htmlAsBytes = System.Text.Encoding.UTF8.GetBytes(html);
            var results = _sut.FindLinks(htmlAsBytes, "somePath");

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("some/path", results.First());
        }

        [TestMethod]
        public void FindLinks_ReturnsCorrectPath_WhenLinkIsRelative()
        {
            var html = "<a href=\"../blog\">Blog</a>";
            var htmlAsBytes = System.Text.Encoding.UTF8.GetBytes(html);

            var results = _sut.FindLinks(htmlAsBytes, "main/about");
            
            Assert.AreEqual("blog", results.First());
        }
    }
}
