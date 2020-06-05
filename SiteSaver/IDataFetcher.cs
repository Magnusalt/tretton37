using System.Threading.Tasks;

namespace SiteSaver
{
    public interface IDataFetcher
    {
        Task<(byte[] Document, string Path)> Fetch(string relativeResource);
    }
}