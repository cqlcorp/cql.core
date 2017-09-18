// ***********************************************************************
// Assembly         : Cql.Core.Owin
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-18-2017
// ***********************************************************************
// <copyright file="HeaderExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Owin
{
    using System.Linq;

    using JetBrains.Annotations;

    using Microsoft.Owin;

    /// <summary>
    /// Class HeaderExtensions.
    /// </summary>
    public static class HeaderExtensions
    {
        /// <summary>
        /// Gets the IP address of client forwarded by a proxy or load-balancer.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <returns><see cref="System.String"/></returns>
        [CanBeNull]
        public static string ForwardedFor([NotNull] this IHeaderDictionary headers)
        {
            return headers.GetSingleHeaderValue("x-forwarded-for");
        }

        /// <summary>
        /// Gets original host name when forwarded by a proxy or load-balancer.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <returns><see cref="System.String"/></returns>
        [CanBeNull]
        public static string ForwardedOrigin([NotNull] this IHeaderDictionary headers)
        {
            return headers.GetSingleHeaderValue("origin") ?? headers.GetSingleHeaderValue("x-forwarded-host");
        }

        /// <summary>
        /// Gets original request port (HTTP/HTTPS) when forwarded by a proxy or load-balancer.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <returns>The port number</returns>
        public static int? ForwardedPort([NotNull] this IHeaderDictionary headers)
        {
            var portValue = headers.GetSingleHeaderValue("x-forwarded-port");

            return int.TryParse(portValue, out var port) ? (int?)port : null;
        }

        /// <summary>
        /// Gets original request Protocol (HTTP/HTTPS) when forwarded by a proxy or load-balancer.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <returns><see cref="System.String"/></returns>
        [CanBeNull]
        public static string ForwardedProtocol([NotNull] this IHeaderDictionary headers)
        {
            return headers.GetSingleHeaderValue("x-forwarded-proto");
        }

        /// <summary>
        /// Gets the single header value.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="key">The key.</param>
        /// <returns>The header value or null.</returns>
        [CanBeNull]
        public static string GetSingleHeaderValue([NotNull] this IHeaderDictionary headers, [NotNull] string key)
        {
            headers.TryGetValue(key, out var values);

            return values?.FirstOrDefault();
        }
    }
}
