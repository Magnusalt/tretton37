using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SiteSaver
{
    public class DiskFileHandler : IFileHandler
    {
        private readonly string _destinationDirectory;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<DiskFileHandler> _logger;

        public DiskFileHandler(string destinationDirectory, IFileSystem fileSystem, ILogger<DiskFileHandler> logger)
        {
            if (!Uri.IsWellFormedUriString(destinationDirectory, UriKind.RelativeOrAbsolute))
            {
                throw new ArgumentException("Destination directory is not in an appropriate format");
            }

            _destinationDirectory = destinationDirectory;
            _fileSystem = fileSystem;
            _logger = logger;

            _fileSystem.CreateDirectory(_destinationDirectory);
        }

        // Stores a file at the path relative to the provided destination directory
        public async Task Store(string relativePath, byte[] fileContent)
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
            _fileSystem.CreateDirectory(directory);
            
            _logger.LogInformation("Writing {0} to disk", filePath);

            await _fileSystem.WriteFileToDisk(filePath, fileContent);

            _logger.LogInformation("Done writing {0} to disk", filePath);
        }
    }
}