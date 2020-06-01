using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SiteSaver
{
    public class HtmlFetcher : IDataFetcher
    {
        private readonly HttpClient _httpClient;
        public HtmlFetcher(Uri baseUri)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;
        }

        public Task<string> Fetch(string relativeResource)
        {
            return _httpClient.GetStringAsync(relativeResource);
        }
    }
}