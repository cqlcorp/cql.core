using System.Net.Http.Headers;

namespace Cql.Core.Web
{
    public static class BasicAuthenticationHelper
    {
        public static AuthenticationHeaderValue CreateBasicAuthHeader(IBasicAuthCredentials credentials)
        {
            return new AuthenticationHeaderValue("Basic", credentials.ToBase64String());
        }
    }
}
