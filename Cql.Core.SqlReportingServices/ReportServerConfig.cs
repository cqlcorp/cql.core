namespace Cql.Core.SqlReportingServices
{
    using System;
    using System.ServiceModel.Channels;

    public class ReportServerConfig : IReportServerConfig
    {
        public ReportServerConfig(string endPointAddress, Binding binding = null)
            : this(new Uri(endPointAddress), binding)
        {
        }

        public ReportServerConfig(Uri endPointAddress, Binding binding = null)
        {
            this.EndPointAddress = endPointAddress;
            this.Binding = binding;
        }

        public Binding Binding { get; set; }

        public Uri EndPointAddress { get; set; }
    }
}
