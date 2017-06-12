using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Cql.Core.TransactionScopes
{
    public static class XactScope
    {
        public static async Task ExecuteAsync(Func<Task> taskToRunInsideTransactionScope, TimeSpan? transactionScopeTimeout = default(TimeSpan?))
        {
            using (var xact = CreateAsyncTransactionScope(transactionScopeTimeout))
            {
                await taskToRunInsideTransactionScope();

                xact.Complete();
            }
        }

        public static async Task<T> ExecuteAsync<T>(Func<Task<T>> taskToRunInsideTransactionScope, TimeSpan? transactionScopeTimeout = default(TimeSpan?))
        {
            using (var xact = CreateAsyncTransactionScope(transactionScopeTimeout))
            {
                var ret = await taskToRunInsideTransactionScope();

                xact.Complete();

                return ret;
            }
        }

        private static TransactionScope CreateAsyncTransactionScope(TimeSpan? transactionScopeTimeout = default (TimeSpan?))
        {
            const TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required;
            const TransactionScopeAsyncFlowOption scopeAsyncFlowOption = TransactionScopeAsyncFlowOption.Enabled;

            return transactionScopeTimeout.HasValue ?
                new TransactionScope(transactionScopeOption, transactionScopeTimeout.Value, scopeAsyncFlowOption) :
                new TransactionScope(transactionScopeOption, scopeAsyncFlowOption);
        }
    }
}
