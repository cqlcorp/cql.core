using System;

using Cql.Core.Common.Extensions;

using Microsoft.Owin;

namespace Cql.Core.Owin.WebPack
{
    public static class ProxyServerUtil
    {
        /// <summary>
        /// Handles passing through the Webpack Proxy server's URL
        /// </summary>
        public static string GetProxyAuthority(IOwinRequest owinRequest)
        {
            var headers = owinRequest.Headers;

            var origin = headers.ForwardedOrigin();
            var port = headers.ForwardedPort();
            var proto = headers.ForwardedProtocol() ?? owinRequest.Uri.Scheme;

            if (string.IsNullOrWhiteSpace(origin) || !port.IsWebpackDevServer())
            {
                return null;
            }

            if (!origin.Contains(":") && port.HasValue && port.Value != 80 && port.Value != 443)
            {
                origin = $"{origin}:{port}";
            }

            if (!origin.Contains("://", StringComparison.OrdinalIgnoreCase))
            {
                origin = $"{proto}://{origin}";
            }

            return origin;
        }
    }
}
