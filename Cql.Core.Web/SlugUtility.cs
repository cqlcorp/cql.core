// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="SlugUtility.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    using JetBrains.Annotations;

    /// <summary>
    /// Class SlugUtility.
    /// </summary>
    public static class SlugUtility
    {
        /// <summary>
        /// The invalid chars
        /// </summary>
        [NotNull]
        private static readonly Regex InvalidChars = new Regex(@"[^a-z0-9\s-]", RegexOptions.Compiled);

        /// <summary>
        /// Generates a string value that can be used a URL slug.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <returns>A URL slug</returns>
        /// <exception cref="System.ArgumentException">The <paramref name="phrase" /> cannot be null or whitespace.</exception>
        [NotNull]
        public static string GenerateSlug([NotNull] string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase))
            {
                throw new ArgumentException("message", nameof(phrase));
            }

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

        /// <summary>
        /// Removes the accent characters.
        /// </summary>
        /// <param name="txt">The text.</param>
        /// <returns>A System.String.</returns>
        [NotNull]
        private static string RemoveAccent([NotNull] this string txt)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
