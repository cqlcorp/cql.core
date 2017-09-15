using System;
using System.Linq;

using Microsoft.AspNetCore.Http;

namespace Cql.Core.AspNetCore.Utils
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Returns the host name, port and scheme portion of the Url
        /// </summary>
        public static string Authority(this HttpRequest request)
        {
            var uri = request.ToUri();

            return $"{uri.Scheme}://{uri.Authority}";
        }

        public static UriBuilder BuildableUri(this HttpRequest request, BuildableUriOptions options = BuildableUriOptions.None)
        {
            var hostComponents = request.Host.ToUriComponent().Split(':');

            var builder = new UriBuilder
                          {
                              Scheme = request.Scheme,
                              Host = hostComponents[0],
                              Path = options.HasFlagFast(BuildableUriOptions.IncludePath) ? request.Path : new PathString(""),
                              Query = options.HasFlagFast(BuildableUriOptions.IncludeQuery) ? request.QueryString.ToUriComponent().TrimStart('?') : ""
                          };

            if (hostComponents.Length == 2)
            {
                builder.Port = Convert.ToInt32(hostComponents[1]);
            }

            if (IsWellKnownPort(builder))
            {
                builder.Port = -1;
            }

            return builder;
        }

        /// <summary>
        /// Returns <c>true</c> if the Accept header contains "application/json"; otherwise <c>false</c>.
        /// </summary>
        public static bool ExpectsJson(this HttpRequest request)
        {
            return request.GetTypedHeaders().Accept?.Any(x => x.MediaType == "application/json") == true;
        }

        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// <returns>
        /// true if the specified HTTP request is an AJAX request; otherwise, false.
        /// </returns>
        /// <param name="request">The HTTP request.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="request" /> parameter is null (Nothing in Visual
        /// Basic).
        /// </exception>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Headers.TryGetValue("X-Requested-With", out var headerValues)
                   && headerValues.Any(value => string.Equals(value, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase));
        }

        public static string PathAndQuery(this HttpRequest request)
        {
            return request.ToUri().PathAndQuery;
        }

        public static Uri ToUri(this HttpRequest request)
        {
            return request.BuildableUri(BuildableUriOptions.IncludePathAndQuery).Uri;
        }

        public static string UserAgent(this HttpRequest request)
        {
            request.Headers.TryGetValue("User-Agent", out var userAgent);

            return userAgent.FirstOrDefault();
        }

        private static bool IsWellKnownPort(UriBuilder builder)
        {
            return builder.Port == 443 || builder.Port == 80;
        }
    }
}
