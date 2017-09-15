namespace Cql.Core.SqlReportingServices
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;

    internal static class BasicAuthHelper
    {
        public static void SetBasicAuthHeader(NetworkCredential credentials)
        {
            if (string.IsNullOrEmpty(credentials?.UserName))
            {
                return;
            }

            var requestProperty = new HttpRequestMessageProperty();
            requestProperty.Headers[HttpRequestHeader.Authorization] = BasicAuthHeader(credentials);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestProperty;
        }

        private static string BasicAuthHeader(NetworkCredential credentials)
        {
            return $"Basic {ToBase64String(credentials)}";
        }

        private static string ToBase64String(NetworkCredential credentials)
        {
            var formattedString = $"{credentials.UserNameWithDomain()}:{credentials.Password}";

            var encodedBytes = Encoding.UTF8.GetBytes(formattedString);

            return Convert.ToBase64String(encodedBytes);
        }
    }
}
