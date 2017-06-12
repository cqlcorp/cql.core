using Cql.Core.ReportingServices.ReportServer;

namespace Cql.Core.SqlReportingServices
{
    public interface IReportServerExecutionContext
    {
        TrustedUserHeader TrustedUserHeader { get; }

        ReportingService2010SoapClient Client { get; }
    }
}
