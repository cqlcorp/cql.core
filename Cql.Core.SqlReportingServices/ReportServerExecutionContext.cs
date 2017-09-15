namespace Cql.Core.SqlReportingServices
{
    using System.Threading;

    using Cql.Core.ReportingServices.ReportServer;

    internal class ReportServerExecutionContext : IReportServerExecutionContext
    {
        private TrustedUserHeader _trustedUserHeader;

        public ReportServerExecutionContext(ReportingService2010SoapClient client)
        {
            this.Client = client;
        }

        public ReportingService2010SoapClient Client { get; }

        public TrustedUserHeader TrustedUserHeader
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref this._trustedUserHeader, () => new TrustedUserHeader { UserName = this.Client.ClientCredentials?.UserName.UserName });
            }
        }
    }
}
