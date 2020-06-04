using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SiteSaver.Tests
{
    [TestClass]
    public class DataFetcherTests
    {
        private IDataFetcher _sut;

        [TestInitialize]
        public void Setup()
        {
            _sut = new RemoteResourceFetcher(new System.Uri("https://tretton37.com"));
        }

        [TestMethod]
        [Ignore("This is an integration test to verify that remote resources can be reached, used for development purpose")]
        public async Task Fetch_ReturnsRequestedResource_VerifiedManuallyWithDebugger()
        {
            var result = await _sut.Fetch("/");
        }
    }
}