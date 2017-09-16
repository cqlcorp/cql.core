// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="HttpClientFactory.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using System;
    using System.Net;
    using System.Net.Http;

    using JetBrains.Annotations;

    /// <summary>
    /// Creates a <see cref="HttpClient" /> that supports Automatic Decompression.
    /// </summary>
    public static class HttpClientFactory
    {
        /// <summary>
        /// Creates an HTTP client that supports automatic decompression handling.
        /// </summary>
        /// <param name="setDefaultsHandler">An action that modifies the created HttpClient</param>
        /// <returns>An HttpClient.</returns>
        [NotNull]
        public static HttpClient CreateHttpClient([CanBeNull] Action<HttpClient> setDefaultsHandler = null)
        {
            var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip };

            var client = new HttpClient(handler);

            setDefaultsHandler?.Invoke(client);

            return client;
        }
    }
}
