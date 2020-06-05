using System.Threading.Tasks;

namespace SiteSaver
{
    public interface IFileSystem
    {
        void CreateDirectory(string path);
        Task WriteFileToDisk(string filePath, byte[] fileContent);
    }
}