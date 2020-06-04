using System.Threading.Tasks;

namespace SiteSaver
{
    public interface IDataFetcher
    {
        Task<(string Document, string Path)> Fetch(string relativeResource);
    }
}