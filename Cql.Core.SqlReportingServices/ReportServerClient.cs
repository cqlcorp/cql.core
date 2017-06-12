using System;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cql.Core.SqlReportingServices
{
    public static class ReportServerClient
    {
        public static Task<TResult> ExecuteAsync<TResult>(IReportServerConfig config,
            Func<IReportServerExecutionContext, Task<TResult>> task)
        {
            return ExecuteAsync(null, config, task);
        }

        public static Task<TResult> ExecuteAsync<TResult>(
            NetworkCredential credentials,
            IReportServerConfig config,
            Func<IReportServerExecutionContext, Task<TResult>> task)
        {
            var client = SoapClientFactory.CreateReportServiceClient(config);

            client.SetCredentials(credentials);

            using (new OperationContextScope(client.InnerChannel))
            {
                if (client.GetCredentialType() == HttpClientCredentialType.Basic)
                {
                    BasicAuthHelper.SetBasicAuthHeader(credentials);
                }

                var context = new ReportServerExecutionContext(client);

                return task(context);
            }
        }

        public static Task ExecuteAsync(IReportServerConfig config,
            Func<IReportServerExecutionContext, Task> task)
        {
            return ExecuteAsync(null, config, task);
        }

        public static Task ExecuteAsync(
            NetworkCredential credentials,
            IReportServerConfig config,
            Func<IReportServerExecutionContext, Task> task)
        {
            var client = SoapClientFactory.CreateReportServiceClient(config);

            client.SetCredentials(credentials);

            using (new OperationContextScope(client.InnerChannel))
            {
                if (client.GetCredentialType() == HttpClientCredentialType.Basic)
                {
                    BasicAuthHelper.SetBasicAuthHeader(credentials);
                }

                var context = new ReportServerExecutionContext(client);

                return task(context);
            }
        }
    }
}
