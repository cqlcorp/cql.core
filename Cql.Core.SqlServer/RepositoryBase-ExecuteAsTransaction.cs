#if !XACT

namespace Cql.Core.SqlServer
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;

    public abstract partial class RepositoryBase
    {
        /// <summary>
        /// Executes the operation wrapped in a transaction that is automatically commited if no errors occur
        /// </summary>
        /// <remarks>
        /// .Net core has not yet implemented System.Transactions yet... but soon:
        /// https://github.com/dotnet/corefx/tree/master/src/System.Transactions
        /// </remarks>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <param name="executeFunc"></param>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        protected virtual async Task<TQueryResult> ExecuteAsTransaction<TQueryResult>(
            Func<DbConnection, DbTransaction, Task<TQueryResult>> executeFunc,
            IsolationLevel? isolationLevel = null)
        {
            using (var db = this.CreateConnection())
            {
                if (db.State != ConnectionState.Open)
                {
                    await db.OpenAsync();
                }

                using (var transaction = isolationLevel.HasValue ? db.BeginTransaction(isolationLevel.Value) : db.BeginTransaction())
                {
                    ExceptionDispatchInfo error = null;

                    var result = default(TQueryResult);

                    try
                    {
                        result = await executeFunc(db, transaction).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        error = ExceptionDispatchInfo.Capture(ex);
                    }
                    finally
                    {
                        if (error == null)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            error.Throw();
                        }
                    }

                    return result;
                }
            }
        }
    }
}

#endif
