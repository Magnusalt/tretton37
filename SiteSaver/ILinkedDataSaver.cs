using System;
using System.Threading.Tasks;

namespace SiteSaver
{
    public interface ILinkedDataSaver
    {
        Task Save();
    }

    public class SiteSaver : ILinkedDataSaver
    {
        private readonly IDataFetcher _dataFetcher;
        private readonly ILinkParser _linkParser;
        private readonly IFileHandler _fileHandler;

        public SiteSaver(IDataFetcher dataFetcher, ILinkParser linkParser, IFileHandler fileHandler)
        {
            _dataFetcher = dataFetcher;
            _linkParser = linkParser;
            _fileHandler = fileHandler;
        }
        public async Task Save()
        {
            var result = await _dataFetcher.Fetch("index.html");
            var foundLinks = _linkParser.FindLinks(result);
            await _fileHandler.Store("index.html", result);
        }
    }
}