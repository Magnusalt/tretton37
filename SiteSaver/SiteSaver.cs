using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace SiteSaver
{
    public class SiteSaver : ILinkedDataSaver
    {
        private readonly IDataFetcher _dataFetcher;
        private readonly ILinkParser _linkParser;
        private readonly IFileHandler _fileHandler;

        // This is to keep track of visited links, the keys in the dictionary is used as a concurrent set.
        private readonly ConcurrentDictionary<string, byte> _linkTracker;

        public SiteSaver(IDataFetcher dataFetcher, ILinkParser linkParser, IFileHandler fileHandler)
        {
            _dataFetcher = dataFetcher;
            _linkParser = linkParser;
            _fileHandler = fileHandler;

            _linkTracker = new ConcurrentDictionary<string, byte>();
        }

        public async Task Save()
        {
            var root = "/";
            await TraverseLinks(new[] { root });
        }

        private async Task TraverseLinks(IList<string> links)
        {
            var storeTasks = new List<Task>();
            var fetchTasks = new List<Task<(string Document, string Path)>>();

            foreach (var link in links)
            {
                fetchTasks.Add(_dataFetcher.Fetch(link));
                _linkTracker.TryAdd(link, 0);
            }

            var fetchResults = await Task.WhenAll(fetchTasks);

            var foundLinks = new List<string>();
            foreach (var item in fetchResults)
            {
                foundLinks.AddRange(_linkParser.FindLinks(item.Document, item.Path));
                storeTasks.Add(_fileHandler.Store(item.Path, item.Document));
            }

            foundLinks = foundLinks.Except(_linkTracker.Keys).ToList();

            if (foundLinks.Count > 0)
            {
                await TraverseLinks(foundLinks);
            }

            await Task.WhenAll(storeTasks);
        }
    }
}