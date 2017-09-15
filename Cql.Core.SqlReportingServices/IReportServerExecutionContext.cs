namespace Cql.Core.SqlReportingServices
{
    using Cql.Core.ReportingServices.ReportServer;

    public interface IReportServerExecutionContext
    {
        ReportingService2010SoapClient Client { get; }

        TrustedUserHeader TrustedUserHeader { get; }
    }
}
