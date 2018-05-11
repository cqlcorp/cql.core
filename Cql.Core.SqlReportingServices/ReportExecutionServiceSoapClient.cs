// ReSharper disable CheckNamespace

namespace Cql.Core.ReportingServices.ReportExecution
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading.Tasks;

    [SuppressMessage("ReSharper", "UnusedVariable")]
    public partial class ReportExecutionServiceSoapClient
    {
        public Task<int> FindStringAsync(string executionId, int startPage, int endPage, string findValue)
        {
            using (var context = this.SetMessageHeaders(executionId))
            {
                return this.FindStringAsync(startPage, endPage, findValue);
            }
        }

        public Task<ExecutionInfo> LoadReportAsync(string Report)
        {
            return this.LoadReportAsync(Report, null);
        }

        public Task<ExecutionInfo> GetExecutionInfoAsync(string executionId)
        {
            using (var context = this.SetMessageHeaders(executionId))
            {
                return this.GetExecutionInfoAsync();
            }
        }

        public Task<Render2Response> Render2Async(string executionId, Render2Request request)
        {
            using (var context = this.SetMessageHeaders(executionId))
            {
                return this.Render2Async(request);
            }
        }

        public Task<RenderResponse> RenderAsync(string executionId, RenderRequest request)
        {
            using (var context = this.SetMessageHeaders(executionId))
            {
                return this.RenderAsync(request);
            }
        }

        public Task<RenderStreamResponse> RenderStreamAsync(string executionId, RenderStreamRequest request)
        {
            using (var context = this.SetMessageHeaders(executionId))
            {
                return this.RenderStreamAsync(request);
            }
        }

        public Task<ExecutionInfo> SetReportParametersAsync(string executionId, IEnumerable<KeyValuePair<string, object>> parameterValues)
        {
            return this.SetReportParametersAsync(executionId, parameterValues.Select(x => new ParameterValue
            {
                Name = x.Key,
                Value = x.Value?.ToString()
            }), null);
        }

        public Task<ExecutionInfo> SetReportParametersAsync(string executionId, IEnumerable<ParameterValue> parameterValues, string parameterLanguage = null)
        {
            using (var context = this.SetMessageHeaders(executionId))
            {
                var parameterValuesArray = parameterValues.ToArray();

                if (string.IsNullOrEmpty(parameterLanguage))
                {
                    parameterLanguage = CultureInfo.CurrentUICulture.Name;
                }

                return this.SetExecutionParametersAsync(parameterValuesArray, parameterLanguage);
            }
        }

        private OperationContextScope SetMessageHeaders(string executionId)
        {
            var context = new OperationContextScope(this.InnerChannel);

            var executionHeaderData = new ExecutionHeader { ExecutionID = executionId };

            OperationContext.Current.OutgoingMessageHeaders.Add(executionHeaderData.CreateMessageHeader());

            return context;
        }
    }
}
