using JetBrains.Annotations;

namespace Cql.Core.Web
{
    using System;
    using System.Text;

    public static class BasicAuthExtensions
    {
        [NotNull]
        public static string ToBase64String([NotNull] this IBasicAuthCredentials credentials)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            var formattedString = $"{credentials.UserName}:{credentials.Password}";

            var encodedBytes = Encoding.UTF8.GetBytes(formattedString);

            return Convert.ToBase64String(encodedBytes);
        }
    }
}
