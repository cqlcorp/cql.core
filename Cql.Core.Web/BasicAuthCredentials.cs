using System.Net;
using System.Net.Http.Headers;

namespace Cql.Core.Web
{
    public class BasicAuthCredentials : IBasicAuthCredentials
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public static implicit operator BasicAuthCredentials(NetworkCredential networkCredential)
        {
            return new BasicAuthCredentials
            {
                UserName = networkCredential.UserName,
                Password = networkCredential.Password
            };
        }

        public static implicit operator AuthenticationHeaderValue(BasicAuthCredentials credentials)
        {
            return BasicAuthenticationHelper.CreateBasicAuthHeader(credentials);
        }
    }
}
