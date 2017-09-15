// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="BasicAuthExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using System;
    using System.Text;

    using JetBrains.Annotations;

    /// <summary>
    /// Class BasicAuthExtensions.
    /// </summary>
    public static class BasicAuthExtensions
    {
        /// <summary>
        /// Creates a base64 encoded value of the specified <paramref name="credentials" /> for us in a Basic Auth header.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>A base64 encoded string</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="credentials" /> cannot be null</exception>
        [NotNull]
        public static string ToBase64String([NotNull] this IBasicAuthCredentials credentials)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            var formattedString = $"{credentials.UserName}:{credentials.Password}";

            var encodedBytes = Encoding.UTF8.GetBytes(formattedString);

            return Convert.ToBase64String(encodedBytes);
        }
    }
}
