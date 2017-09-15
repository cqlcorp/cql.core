namespace Cql.Core.SqlReportingServices
{
    using System.Threading;

    using Cql.Core.ReportingServices.ReportExecution;

    internal class ReportExecutionContext : IReportExecutionContext
    {
        private TrustedUserHeader _trustedUserHeader;

        public ReportExecutionContext(ReportExecutionServiceSoapClient client)
        {
            this.Client = client;
        }

        public ReportExecutionServiceSoapClient Client { get; }

        public TrustedUserHeader TrustedUserHeader
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref this._trustedUserHeader, () => new TrustedUserHeader { UserName = this.Client.ClientCredentials.UserName.UserName });
            }
        }
    }
}
