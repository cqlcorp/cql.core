using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cql.Core.Common.Utils
{
    public static class RandomString
    {
        /// <summary>
        ///     Creates a cryptographically strong sequence of characters.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="allowedChars">The allowed chars.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public static string Create(int length, string allowedChars = "abcdefghjklmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789")
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} cannot be less than zero.");
            }

            if (string.IsNullOrEmpty(allowedChars))
            {
                throw new ArgumentNullException(nameof(allowedChars), $"{nameof(allowedChars)} may not be empty.");
            }

            const int byteSize = 0x100;

            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();

            if (byteSize < allowedCharSet.Length)
            {
                throw new ArgumentException($"{nameof(allowedChars)} may contain no more than {byteSize} characters.");
            }

            using (var rng = RandomNumberGenerator.Create())
            {
                var result = new StringBuilder();
                var buf = new byte[128];

                while (result.Length < length)
                {
                    rng.GetBytes(buf);

                    for (var i = 0; (i < buf.Length) && (result.Length < length); ++i)
                    {
                        var outOfRangeStart = byteSize - byteSize%allowedCharSet.Length;
                        if (outOfRangeStart <= buf[i])
                        {
                            continue;
                        }

                        result.Append(allowedCharSet[buf[i]%allowedCharSet.Length]);
                    }
                }

                return result.ToString();
            }
        }
    }
}
