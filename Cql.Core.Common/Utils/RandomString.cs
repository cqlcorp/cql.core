// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="RandomString.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using JetBrains.Annotations;

    /// <summary>
    /// Class RandomString.
    /// </summary>
    public static class RandomString
    {
        /// <summary>
        /// Creates a cryptographically strong sequence of characters.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="allowedChars">The allowed chars.</param>
        /// <returns>A random string</returns>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="length"/> may not be less than zero</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="allowedChars" /> may not be empty</exception>
        /// <exception cref="ArgumentException">The <paramref name="allowedChars" /> may not contain more than 256 characters</exception>
        [NotNull]
        public static string Create(int length, [NotNull] string allowedChars = "abcdefghjklmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789")
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} cannot be less than zero.");
            }

            if (string.IsNullOrEmpty(allowedChars))
            {
                throw new ArgumentNullException(nameof(allowedChars), $"{nameof(allowedChars)} may not be empty.");
            }

            const int ByteSize = 0x100;

            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();

            if (ByteSize < allowedCharSet.Length)
            {
                throw new ArgumentException($"{nameof(allowedChars)} may contain no more than {ByteSize} characters.");
            }

            using (var rng = RandomNumberGenerator.Create())
            {
                var result = new StringBuilder();
                var buf = new byte[128];

                while (result.Length < length)
                {
                    rng.GetBytes(buf);

                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        var outOfRangeStart = ByteSize - (ByteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i])
                        {
                            continue;
                        }

                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }

                return result.ToString();
            }
        }
    }
}
