using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SiteSaver
{
    public class RemoteResourceFetcher : IDataFetcher
    {
        private readonly HttpClient _httpClient;
        public RemoteResourceFetcher(Uri baseUri)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;
        }
        //todo: update to not get string in order to save binary files
        public async Task<(string Document, string Path)> Fetch(string relativeResource)
        {
            string document = await _httpClient.GetStringAsync(relativeResource);

            return (document, relativeResource);
        }
    }
}