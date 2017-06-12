using System;
using System.Linq;

namespace Cql.Core.Common.Utils
{
    public class Base64File
    {
        public string ContentType { get; set; }

        public byte[] FileContents { get; set; }

        public static Base64File Parse(string base64Content)
        {
            if (string.IsNullOrEmpty(base64Content))
            {
                throw new ArgumentNullException(nameof(base64Content));
            }

            var contentType = ParseContentType(base64Content);

            var bytes = ParseContentBytes(base64Content);

            return new Base64File
            {
                ContentType = contentType,
                FileContents = bytes
            };
        }

        private static byte[] ParseContentBytes(string base64Content)
        {
            var startIndex = base64Content.IndexOf("base64,", StringComparison.OrdinalIgnoreCase) + 7;

            var fileContents = base64Content.Substring(startIndex);

            return Convert.FromBase64String(fileContents);
        }

        private static string ParseContentType(string base64Content)
        {
            var indexOfSemiColon = base64Content.IndexOf(";", StringComparison.OrdinalIgnoreCase);

            var contentTypeLabel = base64Content.Substring(0, indexOfSemiColon);

            return contentTypeLabel.Split(':').Last();
        }

        public override string ToString()
        {
            return $"data:{ContentType};base64,{Convert.ToBase64String(FileContents)}";
        }
    }
}
