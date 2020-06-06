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
            _destinationDirectory = destinationDirectory;
            _fileSystem = fileSystem;
            _logger = logger;

            _fileSystem.CreateDirectory(_destinationDirectory);
        }

        // Stores a file at the path relative to the provided destination directory
        public async Task Store(string relativePath, byte[] fileContent)
        {
            var invalidPath = Path.GetInvalidPathChars();
            var invalidFile = Path.GetInvalidFileNameChars();

            if (Path.DirectorySeparatorChar != '/')
            {
                relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar);
            }

            if (relativePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                relativePath = Path.Combine(relativePath, "index.html").TrimStart(Path.DirectorySeparatorChar);
            }

            var relativePathDirectory = Path.GetDirectoryName(relativePath);
            var relativePathFile = Path.GetFileName(relativePath);

            if (!Path.HasExtension(relativePathFile) && !string.IsNullOrEmpty(relativePathFile))
            {
                relativePathFile = Path.ChangeExtension(relativePathFile, "html");
            }

            relativePathDirectory = string.Join('_', relativePathDirectory.Split(Path.GetInvalidPathChars()));
            relativePathFile = string.Join('_', relativePathFile.Split(Path.GetInvalidFileNameChars()));

            var fullFilePath = Path.Combine(_destinationDirectory, relativePathDirectory, relativePathFile);

            var directory = Path.GetDirectoryName(fullFilePath);
            _fileSystem.CreateDirectory(directory);

            _logger.LogInformation("Writing {0} to disk", fullFilePath);

            try
            {
                await _fileSystem.WriteFileToDisk(fullFilePath, fileContent);
            }
            catch (IOException e)
            {
                _logger.LogError("Failed to write {0} to disk, message: {1}", fullFilePath, e.Message);
                return;
            }

            _logger.LogInformation("Done writing {0} to disk", fullFilePath);
        }
    }
}