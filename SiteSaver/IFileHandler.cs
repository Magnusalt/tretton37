using System.IO;
using System.Threading.Tasks;

public interface IFileHandler
{
    Task Store(string relativePath, byte[] fileContent);
}
