using System;
using System.ServiceModel.Channels;

namespace Cql.Core.SqlReportingServices
{
    public interface IReportServerConfig
    {
        Binding Binding { get; set; }

        Uri EndPointAddress { get; set; }
    }
}
