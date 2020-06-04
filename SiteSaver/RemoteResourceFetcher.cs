using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SiteSaver
{
    public class RemoteResourceFetcher : IDataFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public RemoteResourceFetcher(Uri baseUri, ILogger<RemoteResourceFetcher> logger)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;
            _logger = logger;
        }
        //todo: update to not get string in order to save binary files, also add error handling
        public async Task<(string Document, string Path)> Fetch(string relativeResource)
        {
            _logger.LogInformation("Start download of: {0}{1}", _httpClient.BaseAddress, relativeResource);
            string document = await _httpClient.GetStringAsync(relativeResource);
            _logger.LogInformation("Finished download of: {0}{1}", _httpClient.BaseAddress, relativeResource);

            return (document, relativeResource);
        }
    }
}