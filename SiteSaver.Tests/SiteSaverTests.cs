using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace SiteSaver.Tests
{
    [TestClass]
    public class SiteSaverTests
    {
        private IDataFetcher _dataFetcher; 
        private ILinkParser _linkParser;
        private ILinkedDataSaver _sut;

        [TestInitialize]
        public void Setup()
        {
            _dataFetcher = Substitute.For<IDataFetcher>();
            _linkParser = Substitute.For<ILinkParser>();
            var fileHandler = Substitute.For<IFileHandler>();
            var invalidLinkTag = "@@@@@";
            _sut = new SiteSaver(_dataFetcher, _linkParser, fileHandler, invalidLinkTag);
        }

        [TestMethod]
        public async Task Save_VisitsAllValidLinks_WhenRootDocumentContainsLinks()
        {
            _dataFetcher.Fetch(Arg.Is("/")).Returns(Task.FromResult((new byte[0], "")));
            _linkParser.FindLinks(Arg.Any<byte[]>(), Arg.Is("")).Returns(new string[]{"blog"});
            
            await _sut.Save();

            await _dataFetcher.Received().Fetch(Arg.Is("blog"));
        }
    }
}