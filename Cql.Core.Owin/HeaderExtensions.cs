using System.Linq;

using Microsoft.Owin;

namespace Cql.Core.Owin
{
    public static class HeaderExtensions
    {
        /// <summary>
        /// Gets original host name when forwarded by a proxy or load-balancer.
        /// </summary>
        public static string ForwardedOrigin(this IHeaderDictionary headers)
        {
            return headers.GetSingleHeaderValue("origin") ?? headers.GetSingleHeaderValue("x-forwarded-host");
        }

        /// <summary>
        /// Gets the IP address of client forwarded by a proxy or load-balancer.
        /// </summary>
        public static string ForwardedFor(this IHeaderDictionary headers)
        {
            return headers.GetSingleHeaderValue("x-forwarded-for");
        }

        /// <summary>
        /// Gets original request Protocol (HTTP/HTTPS) when forwarded by a proxy or load-balancer.
        /// </summary>
        public static string ForwardedProtocol(this IHeaderDictionary headers)
        {
            return headers.GetSingleHeaderValue("x-forwarded-proto");
        }

        /// <summary>
        /// Gets original request port (HTTP/HTTPS) when forwarded by a proxy or load-balancer.
        /// </summary>
        public static int? ForwardedPort(this IHeaderDictionary headers)
        {
            var portValue = headers.GetSingleHeaderValue("x-forwarded-port");

            int port;
            return int.TryParse(portValue, out port) ? (int?) port : null;
        }

        public static string GetSingleHeaderValue(this IHeaderDictionary headers, string key)
        {
            string[] values;

            headers.TryGetValue(key, out values);

            return values?.FirstOrDefault();
        }
    }
}
