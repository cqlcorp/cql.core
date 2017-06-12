using System.Threading;

using Cql.Core.ReportingServices.ReportExecution;

namespace Cql.Core.SqlReportingServices
{
    internal class ReportExecutionContext : IReportExecutionContext
    {
        private TrustedUserHeader _trustedUserHeader;

        public ReportExecutionContext(ReportExecutionServiceSoapClient client)
        {
            Client = client;
        }

        public TrustedUserHeader TrustedUserHeader
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref _trustedUserHeader, () => new TrustedUserHeader
                {
                    UserName = Client.ClientCredentials.UserName.UserName
                });
            }
        }

        public ReportExecutionServiceSoapClient Client { get; }
    }
}
