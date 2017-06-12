using System;
using System.Net;
using System.Net.Http;

namespace Cql.Core.Web
{
    public static class HttpClientFactory
    {
        /// <summary>
        /// Creates an HTTP client that supports automatic decompression handling.
        /// </summary>
        public static HttpClient CreateHttpClient()
        {
            return CreateHttpClient(null);
        }

        /// <summary>
        /// Creates an HTTP client that supports automatic decompression handling.
        /// </summary>
        /// <param name="setDefaultsHandler">An action that modifies the created HttpClient</param>
        /// <returns></returns>
        public static HttpClient CreateHttpClient(Action<HttpClient> setDefaultsHandler)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            };

            var client = new HttpClient(handler);

            setDefaultsHandler?.Invoke(client);

            return client;
        }
    }
}
