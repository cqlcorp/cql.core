using System.Threading;

using Cql.Core.ReportingServices.ReportServer;

namespace Cql.Core.SqlReportingServices
{
    internal class ReportServerExecutionContext : IReportServerExecutionContext
    {
        private TrustedUserHeader _trustedUserHeader;

        public ReportServerExecutionContext(ReportingService2010SoapClient client)
        {
            Client = client;
        }

        public TrustedUserHeader TrustedUserHeader
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref _trustedUserHeader, () => new TrustedUserHeader
                {
                    UserName = Client.ClientCredentials?.UserName.UserName
                });
            }
        }

        public ReportingService2010SoapClient Client { get; }
    }
}
