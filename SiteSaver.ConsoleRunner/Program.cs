using System;
using System.IO;
using System.Threading.Tasks;

namespace SiteSaver.ConsoleRunner
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var domain = "https://tretton37.com/";

            var uri = new Uri(domain);

            var destination = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Downloads", uri.Host.Replace(".", string.Empty));

            var saver = new SiteSaver(new RemoteResourceFetcher(uri), new HtmlLinkParser(domain), new DiskFileHandler(destination));
            await saver.Save();
        }
    }
}
