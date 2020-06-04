using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SiteSaver
{
    public class DiskFileHandler : IFileHandler
    {
        private readonly string _destinationDirectory;
        private readonly ILogger<DiskFileHandler> _logger;

        public DiskFileHandler(string destinationDirectory, ILogger<DiskFileHandler> logger)
        {
            if (!Uri.IsWellFormedUriString(destinationDirectory, UriKind.RelativeOrAbsolute))
            {
                throw new ArgumentException("Destination directory is not in an appropriate format");
            }

            Directory.CreateDirectory(destinationDirectory);

            _destinationDirectory = destinationDirectory;
            _logger = logger;
        }

        // Stores a file at the path relative to the provided destination directory
        public async Task Store(string relativePath, string fileContent)
        {
            if (relativePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                relativePath = Path.Combine(relativePath, "index.html").TrimStart(Path.DirectorySeparatorChar);
            }

            var filePath = Path.Combine(_destinationDirectory, relativePath);

            if (!Path.HasExtension(filePath) && !string.IsNullOrEmpty(relativePath))
            {
                filePath = Path.ChangeExtension(filePath, "html");
            }

            var directory = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directory);

            //todo: handle binary files, abstract away the filestream to enable unit testing
            var contentAsBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);
            using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                _logger.LogInformation("Writing {0} to disk, total {1} bytes", filePath, contentAsBytes.Length);
                await fileStream.WriteAsync(contentAsBytes, 0, contentAsBytes.Length);
                _logger.LogInformation("Done writing {0} to disk", filePath);
            }
        }
    }
}