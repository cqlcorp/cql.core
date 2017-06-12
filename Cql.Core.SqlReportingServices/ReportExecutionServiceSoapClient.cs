// ReSharper disable CheckNamespace

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cql.Core.ReportingServices.ReportExecution
{
    [SuppressMessage("ReSharper", "UnusedVariable")]
    public partial class ReportExecutionServiceSoapClient
    {
        public Task<int> FindStringAsync(string executionId, int startPage, int endPage, string findValue)
        {
            using (var context = SetMessageHeaders(executionId))
            {
                return FindStringAsync(startPage, endPage, findValue);
            }
        }

        public Task<ExecutionInfo> GetExecutionInfoAsync(string executionId)
        {
            using (var context = SetMessageHeaders(executionId))
            {
                return GetExecutionInfoAsync();
            }
        }

        public Task<RenderResponse> RenderAsync(string executionId, RenderRequest request)
        {
            using (var context = SetMessageHeaders(executionId))
            {
                return RenderAsync(request);
            }
        }

        public Task<Render2Response> Render2Async(string executionId, Render2Request request)
        {
            using (var context = SetMessageHeaders(executionId))
            {
                return Render2Async(request);
            }
        }

        public Task<RenderStreamResponse> RenderStreamAsync(string executionId, RenderStreamRequest request)
        {
            using (var context = SetMessageHeaders(executionId))
            {
                return RenderStreamAsync(request);
            }
        }

        public Task<ExecutionInfo> SetReportParametersAsync(string executionId, IEnumerable<ParameterValue> parameterValues, string parameterLanguage = null)
        {
            using (var context = SetMessageHeaders(executionId))
            {
                var parameterValuesArray = parameterValues.ToArray();

                if (string.IsNullOrEmpty(parameterLanguage))
                {
                    parameterLanguage = CultureInfo.CurrentUICulture.Name;
                }

                return SetExecutionParametersAsync(parameterValuesArray, parameterLanguage);
            }
        }

        private OperationContextScope SetMessageHeaders(string executionId)
        {
            var context = new OperationContextScope(InnerChannel);

            var executionHeaderData = new ExecutionHeader
            {
                ExecutionID = executionId
            };

            OperationContext.Current.OutgoingMessageHeaders.Add(executionHeaderData.CreateMessageHeader());

            return context;
        }
    }
}
