// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="StringUtil.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    /// <summary>
    /// Class StringUtil.
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// Increments the specified text using the specified <paramref name="mode" />.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="mode">The incrementation mode.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>An incremented version of the specified <paramref name="text" />.</returns>
        /// <exception cref="ArgumentNullException">The text cannot be null</exception>
        [NotNull]
        public static string Increment([NotNull] string text, StringIncrementMode mode, int? maxLength = null)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var textArr = text.ToCharArray();

            if (mode == StringIncrementMode.Sequence)
            {
                return SequenceImpl(textArr, maxLength);
            }

            // Add legal characters
            var characters = new List<char>();

            switch (mode)
            {
                case StringIncrementMode.AlphaNumeric:
                case StringIncrementMode.Numeric:
                    for (var c = '0'; c <= '9'; c++)
                    {
                        characters.Add(c);
                    }

                    break;
            }

            switch (mode)
            {
                case StringIncrementMode.AlphaNumeric:
                case StringIncrementMode.Alpha:
                    for (var c = 'a'; c <= 'z'; c++)
                    {
                        characters.Add(c);
                    }

                    break;
            }

            // Loop from end to beginning
            for (var i = textArr.Length - 1; i >= 0; i--)
            {
                if (textArr[i] == characters.Last())
                {
                    textArr[i] = characters.First();
                }
                else
                {
                    textArr[i] = characters[characters.IndexOf(textArr[i]) + 1];
                    break;
                }
            }

            return new string(textArr);
        }

        /// <summary>
        /// Shortens the specified <paramref name="value" /> to the specified <paramref name="maxLength" /> and appends a ellipses
        /// ("..." by default) to the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="ellipses">The ellipses to be used (... is the default).</param>
        /// <returns>The original value or a shortened version with the specified <paramref name="ellipses" /></returns>
        [CanBeNull]
        public static string Shorten([CanBeNull] string value, int maxLength, [CanBeNull] string ellipses = null)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= maxLength)
            {
                return value;
            }

            if (string.IsNullOrEmpty(ellipses))
            {
                ellipses = "...";
            }

            var indexOfNextSpace = value.LastIndexOf(" ", maxLength, StringComparison.Ordinal);

            var text = value.Substring(0, indexOfNextSpace > 0 ? indexOfNextSpace : maxLength).Trim();

            return $"{text}{ellipses}";
        }

        /// <summary>
        /// The same as <see cref="Shorten"/> but wrapped in a span tag with a title attribute containing the original text value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="ellipses">The ellipses to be used (... is the default).</param>
        /// <returns>The original value or a shortened version with the specified <paramref name="ellipses" /></returns>
        [CanBeNull]
        public static string ShortenHtml([CanBeNull] string value, int maxLength, [CanBeNull] string ellipses = null)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= maxLength)
            {
                return value;
            }

            return $"<span title=\"{value}\">{Shorten(value, maxLength, ellipses)}</span>";
        }

        /// <summary>
        /// The sequencing implementation.
        /// </summary>
        /// <param name="textArr">The text arr.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>A sequenced string</returns>
        [NotNull]
        private static string SequenceImpl([NotNull] char[] textArr, [CanBeNull] int? maxLength)
        {
            if (textArr.Length == 0)
            {
                return "1";
            }

            var digits = new Stack<char>();

            while (textArr.Length > 0 && char.IsDigit(textArr.Last()))
            {
                digits.Push(textArr.Last());
                Array.Resize(ref textArr, textArr.Length - 1);
            }

            var digitsArray = digits.ToArray();

            var number = new string(digitsArray);

            int.TryParse(number, out var value);

            var incrementValue = Convert.ToString(value + 1);

            var totalLength = incrementValue.Length + textArr.Length;

            if (maxLength.HasValue && totalLength > maxLength.Value)
            {
                Array.Resize(ref textArr, textArr.Length - (totalLength - maxLength.Value));
            }

            return $"{new string(textArr)}{incrementValue}";
        }
    }
}
