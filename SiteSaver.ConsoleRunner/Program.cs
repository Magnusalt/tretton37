using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SiteSaver.ConsoleRunner
{
    public class Program
    {
        /// <summary>
        /// This app will recursivly download a website
        /// </summary>
        /// <param name="domain">The address of the site to download</param>
        /// <param name="destination">Where to save the files, default [workingDir]\[domainname]</param>
        public static async Task Main(string destination, string domain = "https://tretton37.com/")
        {
            try
            {
                const string InvalidResourceTag = "@@invalid@@";
                var uri = new Uri(domain);

                if (string.IsNullOrEmpty(destination))
                {
                    destination = Path.Combine(Environment.CurrentDirectory, uri.Host.Replace(".", string.Empty));
                }

                var serviceProvider = new ServiceCollection()
                    .AddLogging(log => log.AddConsole())
                    .AddSingleton<IFileSystem, FileSystem>()
                    .AddSingleton<IDataFetcher>(sp=> new RemoteResourceFetcher(uri, sp.GetService<ILogger<RemoteResourceFetcher>>(), InvalidResourceTag))
                    .AddSingleton<ILinkParser>(sp => new HtmlLinkParser(domain))
                    .AddSingleton<IFileHandler>(sp=> new DiskFileHandler(destination, sp.GetService<IFileSystem>(), sp.GetService<ILogger<DiskFileHandler>>()))
                    .AddSingleton<ILinkedDataSaver>(sp=>new SiteSaver(sp.GetService<IDataFetcher>(), sp.GetService<ILinkParser>(), sp.GetService<IFileHandler>(), InvalidResourceTag))
                    .BuildServiceProvider();

                var saver = serviceProvider.GetService<ILinkedDataSaver>();
                await saver.Save();

                var logger = serviceProvider.GetService<ILogger<Program>>();

                logger.LogInformation("Finished downloading {0}, saved files to: {1}", domain, destination);
                logger.LogInformation("Press enter to exit.");
                Console.ReadLine();
            }
            catch (UriFormatException)
            {
                System.Console.WriteLine("The domain adress format was not valid, did you forget the protocol (http, https)?");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
