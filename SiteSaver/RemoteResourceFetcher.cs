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
        private readonly string _invalidResourceTag;

        public RemoteResourceFetcher(Uri baseUri, ILogger<RemoteResourceFetcher> logger, string invalidResourceTag)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;
            _logger = logger;
            _invalidResourceTag = invalidResourceTag;
        }

        public async Task<(byte[] Document, string Path)> Fetch(string relativeResource)
        {
            try
            {
                _logger.LogInformation("Start download of: {0}{1}", _httpClient.BaseAddress, relativeResource);
                var response = await _httpClient.GetAsync(relativeResource);
                response.EnsureSuccessStatusCode();

                _logger.LogInformation("Finished download of: {0}{1}", _httpClient.BaseAddress, relativeResource);

                return (await response.Content.ReadAsByteArrayAsync(), relativeResource);

            }
            catch (HttpRequestException e)
            {
                _logger.LogError("Failed when requesting {0}, error: {1}", relativeResource, e.Message);
                return (null, _invalidResourceTag);
            }
        }
    }
}