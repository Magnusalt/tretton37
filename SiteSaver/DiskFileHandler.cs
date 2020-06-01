using System;
using System.IO;
using System.Threading.Tasks;
namespace SiteSaver
{
    public class DiskFileHandler : IFileHandler
    {
        private readonly string _destinationDirectory;
        public DiskFileHandler(string destinationDirectory)
        {
            if (!Uri.IsWellFormedUriString(destinationDirectory, UriKind.RelativeOrAbsolute))
            {
                throw new ArgumentException("Destination directory is not in an appropriate format");
            }

            Directory.CreateDirectory(destinationDirectory);

            _destinationDirectory = destinationDirectory;
        }

        // Stores a file at the path relative to the provided destination directory
        public async Task Store(string relativePath, string fileContent)
        {
            var contentAsBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);
            var filePath = Path.Combine(_destinationDirectory, relativePath);
            var directory = Path.GetDirectoryName(filePath);

            Directory.CreateDirectory(directory);

            using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                await fileStream.WriteAsync(contentAsBytes, 0, contentAsBytes.Length);
            }
        }
    }
}