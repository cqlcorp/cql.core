using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

using Cql.Core.Web;

namespace Cql.Core.Owin
{
    public static class ExtensionsForUri
    {
        /// <summary>
        /// Returns a <see cref="UriBuilder" /> created from the Authority portion of the <paramref name="uri" />.
        /// </summary>
        public static UriBuilder Buildable(this Uri uri, string authority = null, UseSslOption sslOption = UseSslOption.UseCurrentRequest)
        {
            if (string.IsNullOrWhiteSpace(authority))
            {
                authority = uri.GetLeftPart(UriPartial.Authority);
            }

            var localUri = new UriBuilder(authority);

            string scheme;

            switch (sslOption)
            {
                case UseSslOption.Never:
                    scheme = Uri.UriSchemeHttp;
                    break;
                case UseSslOption.Always:
                    scheme = Uri.UriSchemeHttps;
                    break;
                default:
                    scheme = localUri.Scheme;
                    break;
            }

            if (localUri.IsDefaultPort())
            {
                localUri.Port = -1;
            }

            return new UriBuilder(scheme, localUri.Host, localUri.Port);
        }

        public static UriBuilder ChangeSubDomain(this UriBuilder builder, string domain)
        {
            if (!builder.IsLocalhost())
            {
                var currentSubDomain = WebUtil.GetSubDomain(builder.Uri);

                builder.Host = builder.Host.Replace(currentSubDomain, domain);
            }

            return builder;
        }

        public static bool IsDefaultPort(this Uri uri)
        {
            return IsDefaultPort(uri?.Port);
        }

        public static bool IsDefaultPort(this UriBuilder uri)
        {
            return IsDefaultPort(uri?.Port);
        }

        public static bool IsLocalhost(this UriBuilder uri)
        {
            return IsLocalhost(uri?.Host);
        }

        public static bool IsLocalhost(this Uri uri)
        {
            return IsLocalhost(uri?.Host);
        }

        public static string ToQueryString(this NameValueCollection nvc)
        {
            return string.Join(
                "&",
                from key in nvc.AllKeys
                from value in nvc.GetValues(key)
                select $"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(value)}");
        }

        public static string ToUncPath(this Uri uri)
        {
            var builder = new UriBuilder(uri)
            {
                Scheme = Uri.UriSchemeFile,
                Port = -1
            };

            return builder.Uri.LocalPath;
        }

        public static string ToUriString(this UriBuilder builder)
        {
            if (builder.IsDefaultPort())
            {
                builder.Port = -1;
            }

            return builder.Uri.ToString();
        }

        private static bool IsDefaultPort(int? port)
        {
            return port.GetValueOrDefault() == 80 || port.GetValueOrDefault() == 443;
        }

        private static bool IsLocalhost(string host)
        {
            return string.Equals("localhost", host, StringComparison.OrdinalIgnoreCase);
        }
    }
}
