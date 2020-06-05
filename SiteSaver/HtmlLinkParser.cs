using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteSaver
{
    public class HtmlLinkParser : ILinkParser
    {
        private readonly string _domain;

        public HtmlLinkParser(string domain)
        {
            _domain = domain;
        }

        public string[] FindLinks(byte[] file, string path)
        {
            if (Path.HasExtension(path))
            {
                return new string[0];
            }
            // Regex pattern from here: https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-example-scanning-for-hrefs
            string hrefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";

            var html = System.Text.Encoding.UTF8.GetString(file);
            var matchResult = Regex.Matches(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return matchResult
                    .Cast<Match>()
                    .Select(m => m.Groups[1].Value.Trim())
                    .Select(CleanLinks)
                    .Where(FilterInvalidLinks)
                    .Where(FilterRemoteDomains)
                    .Select(l => AdjustToSource(l, path))
                    .Distinct()
                    .ToArray();
        }

        private string AdjustToSource(string link, string path)
        {
            if (!link.StartsWith(".."))
            {
                return link;
            }

            int level = 0;
            while (link.Substring(level * 3, 3) == $"..{Path.DirectorySeparatorChar}")
            {
                level++;
            }
            var pathComponents = path.Split(Path.DirectorySeparatorChar);
            int levelsToKeep = pathComponents.Length - level - 1;

            var finalComp = pathComponents.Take(levelsToKeep).Append(link.Substring(level * 3));

            var absolutePath = Path.Combine(finalComp.ToArray());
            return absolutePath;
        }

        // These invalid beginings are problem specific, consider adding posibility to send in other as arguments
        private bool FilterInvalidLinks(string link)
        {
            if (link.StartsWith("javascript")
                || link.StartsWith("mailto")
                || link.StartsWith("tel")
                || link.StartsWith("sms"))
            {
                return false;
            }
            if (string.IsNullOrEmpty(link))
            {
                return false;
            }
            return true;
        }

        private string CleanLinks(string match)
        {
            if (match.StartsWith("/"))
            {
                match = match.TrimStart('/');
            }

            if (Path.GetExtension(match).ToLowerInvariant() == ".html")
            {
                match = Path.ChangeExtension(match, null);
            }

            return match.Contains('#') ? match.Split('#')[0] : match;
        }

        private bool FilterRemoteDomains(string link)
        {
            if (!link.StartsWith("http"))
            {
                return true;
            }

            return link.Contains(_domain);
        }
    }
}
