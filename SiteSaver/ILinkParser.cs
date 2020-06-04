namespace SiteSaver
{
    public interface ILinkParser
    {
        string[] FindLinks(string html, string path);
    }
}