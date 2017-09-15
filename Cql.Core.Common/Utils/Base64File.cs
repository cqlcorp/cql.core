// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="Base64File.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Utils
{
    using System;
    using System.Linq;

    /// <summary>
    /// Class Base64File.
    /// </summary>
    public class Base64File
    {
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the file contents.
        /// </summary>
        /// <value>The file contents.</value>
        public byte[] FileContents { get; set; }

        /// <summary>
        /// Parses the specified base64 content.
        /// </summary>
        /// <param name="base64Content">Content of the base64.</param>
        /// <returns>Base64File.</returns>
        /// <exception cref="ArgumentNullException">base64Content</exception>
        public static Base64File Parse(string base64Content)
        {
            if (string.IsNullOrEmpty(base64Content))
            {
                throw new ArgumentNullException(nameof(base64Content));
            }

            var contentType = ParseContentType(base64Content);

            var bytes = ParseContentBytes(base64Content);

            return new Base64File { ContentType = contentType, FileContents = bytes };
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"data:{this.ContentType};base64,{Convert.ToBase64String(this.FileContents)}";
        }

        /// <summary>
        /// Parses the content bytes.
        /// </summary>
        /// <param name="base64Content">Content of the base64.</param>
        /// <returns>System.Byte[].</returns>
        private static byte[] ParseContentBytes(string base64Content)
        {
            var startIndex = base64Content.IndexOf("base64,", StringComparison.OrdinalIgnoreCase) + 7;

            var fileContents = base64Content.Substring(startIndex);

            return Convert.FromBase64String(fileContents);
        }

        /// <summary>
        /// Parses the type of the content.
        /// </summary>
        /// <param name="base64Content">Content of the base64.</param>
        /// <returns>System.String.</returns>
        private static string ParseContentType(string base64Content)
        {
            var indexOfSemiColon = base64Content.IndexOf(";", StringComparison.OrdinalIgnoreCase);

            var contentTypeLabel = base64Content.Substring(0, indexOfSemiColon);

            return contentTypeLabel.Split(':').Last();
        }
    }
}
