namespace Cql.Core.SqlReportingServices
{
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.ServiceModel.Security;

    using Cql.Core.ReportingServices.ReportExecution;
    using Cql.Core.ReportingServices.ReportServer;

    internal static class SoapClientFactory
    {
        public static ReportExecutionServiceSoapClient CreateReportExecutionClient(IReportServerConfig config)
        {
            InitBinding(config);
            return new ReportExecutionServiceSoapClient(config.Binding, new EndpointAddress(config.EndPointAddress));
        }

        public static ReportingService2010SoapClient CreateReportServiceClient(IReportServerConfig config)
        {
            InitBinding(config);
            return new ReportingService2010SoapClient(config.Binding, new EndpointAddress(config.EndPointAddress))
                       {
                           ClientCredentials =
                               {
                                   ServiceCertificate =
                                       {
                                           SslCertificateAuthentication
                                               = new
                                                     X509ServiceCertificateAuthentication
                                                         {
                                                             CertificateValidationMode
                                                                 = X509CertificateValidationMode
                                                                     .None,
                                                             RevocationMode
                                                                 = X509RevocationMode
                                                                     .NoCheck
                                                         }
                                       }
                               }
                       };
        }

        private static void InitBinding(IReportServerConfig config)
        {
            if (config.Binding != null)
            {
                return;
            }

            config.Binding = IsEndPointSecure(config) ? ClientUtils.NtlmSecureTransportBinding() : ClientUtils.NtlmTransportBinding();
        }

        private static bool IsEndPointSecure(IReportServerConfig config)
        {
            return config.EndPointAddress.Scheme.StartsWith("https");
        }
    }
}
