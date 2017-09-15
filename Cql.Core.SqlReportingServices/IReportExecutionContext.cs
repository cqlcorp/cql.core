namespace Cql.Core.SqlReportingServices
{
    using Cql.Core.ReportingServices.ReportExecution;

    public interface IReportExecutionContext
    {
        ReportExecutionServiceSoapClient Client { get; }

        TrustedUserHeader TrustedUserHeader { get; }
    }
}
