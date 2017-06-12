using Cql.Core.ReportingServices.ReportExecution;

namespace Cql.Core.SqlReportingServices
{
    public interface IReportExecutionContext
    {
        TrustedUserHeader TrustedUserHeader { get; }

        ReportExecutionServiceSoapClient Client { get; }
    }
}
