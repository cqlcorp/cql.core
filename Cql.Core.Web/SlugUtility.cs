namespace Cql.Core.Web
{
    using System.Text;
    using System.Text.RegularExpressions;

    public static class SlugUtility
    {
        private static readonly Regex InvalidChars = new Regex(@"[^a-z0-9\s-]", RegexOptions.Compiled);

        public static string GenerateSlug(string phrase)
        {
            var str = phrase.RemoveAccent().ToLower();

            // invalid chars
            str = InvalidChars.Replace(str, string.Empty);

            // convert multiple spaces into one space
            str = Regex.Replace(str, @"\s+", " ").Trim();

            // cut and trim
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens

            return str;
        }

        private static string RemoveAccent(this string txt)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
