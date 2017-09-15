using System;

using JetBrains.Annotations;

namespace Cql.Core.Web
{
    using System.Net.Http.Headers;

    public static class BasicAuthenticationHelper
    {
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
