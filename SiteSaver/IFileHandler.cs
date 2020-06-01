using System.Threading.Tasks;

public interface IFileHandler
{
    Task Store(string relativePath, string fileContent);
}
