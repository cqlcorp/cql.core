namespace Cql.Core.Messaging
{
    using System.Text.RegularExpressions;

    public class HtmlUtils
    {
        private static readonly Regex HtmlDetectionRegex = new Regex("<(.*\\s*)>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsHtml(string text)
        {
            return !string.IsNullOrEmpty(text) && HtmlDetectionRegex.IsMatch(text);
        }
    }
}
