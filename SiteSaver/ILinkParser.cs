namespace SiteSaver
{
    public interface ILinkParser
    {
        string[] FindLinks(byte[] file, string path);
    }
}