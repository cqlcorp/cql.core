namespace Cql.Core.SqlReportingServices
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;
    using System.Threading.Tasks;

    public static class ReportServerClient
    {
        public static Task<TResult> ExecuteAsync<TResult>(IReportServerConfig config, Func<IReportServerExecutionContext, Task<TResult>> task)
        {
            return ExecuteAsync(config, task, CancellationToken.None);
        }

        public static Task<TResult> ExecuteAsync<TResult>(IReportServerConfig config, Func<IReportServerExecutionContext, Task<TResult>> task, CancellationToken cancellationToken)
        {
            return ExecuteAsync(null, config, task, cancellationToken);
        }

        public static Task<TResult> ExecuteAsync<TResult>(NetworkCredential credentials, IReportServerConfig config, Func<IReportServerExecutionContext, Task<TResult>> task)
        {
            return ExecuteAsync(credentials, config, task, CancellationToken.None);
        }

        public static Task<TResult> ExecuteAsync<TResult>(
            NetworkCredential credentials,
            IReportServerConfig config,
            Func<IReportServerExecutionContext, Task<TResult>> task,
            CancellationToken cancellationToken)
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

                return task(context).WithCancellation(cancellationToken);
            }
        }

        public static Task ExecuteAsync(IReportServerConfig config, Func<IReportServerExecutionContext, Task> task)
        {
            return ExecuteAsync(config, task, CancellationToken.None);
        }

        public static Task ExecuteAsync(IReportServerConfig config, Func<IReportServerExecutionContext, Task> task, CancellationToken cancellationToken)
        {
            return ExecuteAsync(null, config, task, cancellationToken);
        }

        public static Task ExecuteAsync(NetworkCredential credentials, IReportServerConfig config, Func<IReportServerExecutionContext, Task> task)
        {
            return ExecuteAsync(credentials, config, task, CancellationToken.None);
        }

        public static Task ExecuteAsync(
            NetworkCredential credentials,
            IReportServerConfig config,
            Func<IReportServerExecutionContext, Task> task,
            CancellationToken cancellationToken)
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

                return task(context).WithCancellation(cancellationToken);
            }
        }
    }
}
