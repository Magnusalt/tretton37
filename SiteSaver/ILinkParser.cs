using System.Collections.Generic;

namespace SiteSaver
{
    public interface ILinkParser
    {
        IEnumerable<string> FindLinks(string html);
    }
}