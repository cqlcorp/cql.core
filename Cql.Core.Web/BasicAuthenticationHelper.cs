// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="BasicAuthenticationHelper.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using System;
    using System.Net.Http.Headers;

    using JetBrains.Annotations;

    /// <summary>
    /// Class BasicAuthenticationHelper.
    /// </summary>
    public static class BasicAuthenticationHelper
    {
        /// <summary>
        /// Creates the basic authentication header.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>A basic auth header.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="credentials" /> cannot be null</exception>
        [NotNull]
        public static AuthenticationHeaderValue CreateBasicAuthHeader([NotNull] IBasicAuthCredentials credentials)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            return new AuthenticationHeaderValue("Basic", credentials.ToBase64String());
        }
    }
}
