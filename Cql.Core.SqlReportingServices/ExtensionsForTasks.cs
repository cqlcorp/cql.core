namespace Cql.Core.SqlReportingServices
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class ExtensionsForTasks
    {
        public static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
        {
            if (cancellationToken == CancellationToken.None)
            {
                await task;
                return;
            }

            var tcs = new TaskCompletionSource<bool>();

            using (cancellationToken.Register(s => (s as TaskCompletionSource<bool>)?.TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }

            await task.ConfigureAwait(false);
        }

        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            if (cancellationToken == CancellationToken.None)
            {
                return await task;
            }

            var tcs = new TaskCompletionSource<bool>();

            using (cancellationToken.Register(s => (s as TaskCompletionSource<bool>)?.TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }

            return await task.ConfigureAwait(false);
        }
    }
}
