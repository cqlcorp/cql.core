using System;
using System.Collections.Generic;
using System.Linq;

namespace Cql.Core.Common.Utils
{
    public static class StringUtil
    {
        /// <summary>
        /// Shortens the specified <paramref name="value" /> to the specified <paramref name="maxLength" /> and appends a ellipses
        /// ("..." by default) to the string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <param name="ellipses"></param>
        /// <returns></returns>
        public static string Shorten(string value, int maxLength, string ellipses = null)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= maxLength)
            {
                return value;
            }

            if (string.IsNullOrEmpty(ellipses))
            {
                ellipses = "...";
            }

            var iNextSpace = value.LastIndexOf(" ", maxLength, StringComparison.Ordinal);
            var text = value.Substring(0, iNextSpace > 0 ? iNextSpace : maxLength).Trim();
            return $"{text}{ellipses}";
        }

        public static string ShortenHtml(string value, int maxLength, string ellipses = null)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= maxLength)
            {
                return value;
            }

            return $"<span title=\"{value}\">{Shorten(value, maxLength, ellipses)}</span>";
        }

        public static string Increment(string text, StringIncrementMode mode, int? maxLength = null)
        {
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

        private static string SequenceImpl(char[] textArr, int? maxLength)
        {
            if (textArr == null || textArr.Length == 0)
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

            int value;

            int.TryParse(number, out value);

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
