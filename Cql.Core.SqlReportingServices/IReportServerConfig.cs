namespace Cql.Core.SqlReportingServices
{
    using System;
    using System.ServiceModel.Channels;

    public interface IReportServerConfig
    {
        Binding Binding { get; set; }

        Uri EndPointAddress { get; set; }
    }
}
