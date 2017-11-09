namespace Cql.Core.SqlReportingServices
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public static class ReportExecutionClient
    {
        public static Task<TResult> ExecuteAsync<TResult>(IReportServerConfig config, Func<IReportExecutionContext, Task<TResult>> task)
        {
            return ExecuteAsync(config, task, CancellationToken.None);
        }

        public static Task<TResult> ExecuteAsync<TResult>(IReportServerConfig config, Func<IReportExecutionContext, Task<TResult>> task, CancellationToken cancellationToken)
        {
            return ExecuteAsync(null, config, task, cancellationToken);
        }

        public static Task<TResult> ExecuteAsync<TResult>(NetworkCredential credentials, IReportServerConfig config, Func<IReportExecutionContext, Task<TResult>> task)
        {
            return ExecuteAsync(credentials, config, task, CancellationToken.None);
        }

        public static async Task<TResult> ExecuteAsync<TResult>(NetworkCredential credentials, IReportServerConfig config, Func<IReportExecutionContext, Task<TResult>> task, CancellationToken cancellationToken)
        {
            var client = SoapClientFactory.CreateReportExecutionClient(config);

            client.SetCredentials(credentials);

            return await task(new ReportExecutionContext(client)).WithCancellation(cancellationToken);
        }

        public static Task ExecuteAsync(IReportServerConfig config, Func<IReportExecutionContext, Task> task)
        {
            return ExecuteAsync(config, task, CancellationToken.None);
        }

        public static Task ExecuteAsync(IReportServerConfig config, Func<IReportExecutionContext, Task> task, CancellationToken cancellationToken)
        {
            return ExecuteAsync(null, config, task, cancellationToken);
        }

        public static Task ExecuteAsync(NetworkCredential credentials, IReportServerConfig config, Func<IReportExecutionContext, Task> task)
        {
            return ExecuteAsync(credentials, config, task, CancellationToken.None);
        }

        public static async Task ExecuteAsync(NetworkCredential credentials, IReportServerConfig config, Func<IReportExecutionContext, Task> task, CancellationToken cancellationToken)
        {
            var client = SoapClientFactory.CreateReportExecutionClient(config);

            client.SetCredentials(credentials);

            await task(new ReportExecutionContext(client)).WithCancellation(cancellationToken);
        }
    }
}
