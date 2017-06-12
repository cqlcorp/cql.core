using System;
using System.Net;
using System.Threading.Tasks;

namespace Cql.Core.SqlReportingServices
{
    public static class ReportExecutionClient
    {
        public static Task<TResult> ExecuteAsync<TResult>(IReportServerConfig config, Func<IReportExecutionContext, Task<TResult>> task)
        {
            return ExecuteAsync(null, config, task);
        }

        public static async Task<TResult> ExecuteAsync<TResult>(NetworkCredential credentials, IReportServerConfig config, Func<IReportExecutionContext, Task<TResult>> task)
        {
            var client = SoapClientFactory.CreateReportExecutionClient(config);

            client.SetCredentials(credentials);

            return await task(new ReportExecutionContext(client));
        }

        public static Task ExecuteAsync(IReportServerConfig config, Func<IReportExecutionContext, Task> task)
        {
            return ExecuteAsync(null, config, task);
        }

        public static async Task ExecuteAsync(NetworkCredential credentials, IReportServerConfig config, Func<IReportExecutionContext, Task> task)
        {
            var client = SoapClientFactory.CreateReportExecutionClient(config);

            client.SetCredentials(credentials);

            await task(new ReportExecutionContext(client));
        }
    }
}
