using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SiteSaver
{
    public class HtmlLinkParser : ILinkParser
    {
        private readonly string[] _blacklistedLinks = { "/javascript:void(0)" };
        private readonly string _domain;

        public HtmlLinkParser(string domain)
        {
            _domain = domain;
        }

        public IEnumerable<string> FindLinks(string html)
        {
            // Regex pattern from here: https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-example-scanning-for-hrefs
            string hrefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";

            var matchResult = Regex.Matches(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return matchResult.Cast<Match>().Select(CleanLinks).Except(_blacklistedLinks).Where(FilterRemoteDomains).Distinct().ToList();
        }

        private string CleanLinks(Match matchedLink)
        {
            var match = matchedLink.Groups[1].Value;

            if (!match.StartsWith("/") && !match.StartsWith("http"))
            {
                match = $"/{match}";
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
