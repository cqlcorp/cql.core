using System;
using System.ServiceModel.Channels;

namespace Cql.Core.SqlReportingServices
{
    public class ReportServerConfig : IReportServerConfig
    {
        public ReportServerConfig(string endPointAddress, Binding binding = null)
            : this(new Uri(endPointAddress), binding)
        {
        }

        public ReportServerConfig(Uri endPointAddress, Binding binding = null)
        {
            EndPointAddress = endPointAddress;
            Binding = binding;
        }

        public Uri EndPointAddress { get; set; }

        public Binding Binding { get; set; }
    }
}
