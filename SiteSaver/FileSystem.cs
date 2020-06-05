using System.IO;
using System.Threading.Tasks;

namespace SiteSaver
{
    public class FileSystem : IFileSystem
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public async Task WriteFileToDisk(string filePath, byte[] fileContent)
        {
            using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                await fileStream.WriteAsync(fileContent, 0, fileContent.Length);
            }
        }
    }
}