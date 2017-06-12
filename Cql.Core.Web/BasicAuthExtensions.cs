using System;
using System.Text;

namespace Cql.Core.Web
{
    public static class BasicAuthExtensions
    {
        public static string ToBase64String(this IBasicAuthCredentials credentials)
        {
            string formattedString = $"{credentials.UserName}:{credentials.Password}";

            var encodedBytes = Encoding.UTF8.GetBytes(formattedString);

            return Convert.ToBase64String(encodedBytes);
        }
    }
}