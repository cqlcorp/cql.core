// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="ExtensionsForHttpClient.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    /// Class ExtensionsForHttpClient.
    /// </summary>
    public static class ExtensionsForHttpClient
    {
        /// <summary>
        /// Writes debug output of the Request to a string similar to:
        /// <para>POST /api/upload HTTP/1.1</para>
        /// <para>Host: localhost</para>
        /// <para>Content-Type: multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW</para>
        /// <para></para>
        /// <para>------WebKitFormBoundary7MA4YWxkTrZu0gW</para>
        /// <para>Content-Disposition: form-data; name="files[]"; filename=""</para>
        /// <para>Content-Type: </para>
        /// <para></para>
        /// <para>------WebKitFormBoundary7MA4YWxkTrZu0gW--</para>
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A string similar to a HTTP request</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="request" /> cannot be null</exception>
        public static async Task<string> ToDebugHttpStringAsync([NotNull] this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var sw = new StringWriter())
            {
                await sw.WriteAsync("\n");
                await sw.WriteLineAsync($"{request.Method.Method} {request.RequestUri.PathAndQuery} HTTP/1.1");
                await sw.WriteLineAsync($"Host: {request.RequestUri.Host}");

                await PrintAllHeaders(request, sw);

                await sw.WriteAsync("\n");
                await sw.WriteLineAsync(await request.Content.ReadAsStringAsync());

                return sw.ToString();
            }
        }

        /// <summary>
        /// Prints all headers.
        /// </summary>
        /// <param name="request">The request instance.</param>
        /// <param name="sw">The text writer.</param>
        /// <returns>A task</returns>
        private static async Task PrintAllHeaders([NotNull] HttpRequestMessage request, [NotNull] TextWriter sw)
        {
            foreach (var header in request.Headers)
            {
                await sw.WriteLineAsync($"{header.Key}: {string.Join(",", header.Value)}");
            }

            foreach (var header in request.Content.Headers)
            {
                await sw.WriteLineAsync($"{header.Key}: {string.Join(",", header.Value)}");
            }
        }
    }
}
