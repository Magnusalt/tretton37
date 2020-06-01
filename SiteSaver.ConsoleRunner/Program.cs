using System;
using System.IO;
using System.Threading.Tasks;

namespace SiteSaver.ConsoleRunner
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var uri = "https://tretton37.com";
            var destination = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "tretton");

            var saver = new SiteSaver(new HtmlFetcher(new Uri(uri)), new HtmlLinkParser(uri), new DiskFileHandler(destination));
            await saver.Save();
        }
    }
}
