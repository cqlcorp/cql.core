using System.Diagnostics.Contracts;

using JetBrains.Annotations;

namespace Cql.Core.Web
{
    using System.Net;
    using System.Net.Http.Headers;

    public class BasicAuthCredentials : IBasicAuthCredentials
    {
        public string Password { get; set; }

        public string UserName { get; set; }

        [NotNull]
        public static implicit operator BasicAuthCredentials([CanBeNull] NetworkCredential networkCredential)
        {
            return new BasicAuthCredentials { UserName = networkCredential?.UserName, Password = networkCredential?.Password };
        }

        public static implicit operator AuthenticationHeaderValue(BasicAuthCredentials credentials)
        {
            return BasicAuthenticationHelper.CreateBasicAuthHeader(credentials);
        }
    }
}
