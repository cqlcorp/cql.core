// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-16-2017
// ***********************************************************************
// <copyright file="RepositoryBase-ExecuteAsTransaction.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

#if !XACT

namespace Cql.Core.SqlServer
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    /// Class RepositoryBase.
    /// </summary>
    /// <seealso cref="Cql.Core.SqlServer.IDbConnectionCreator" />
    public abstract partial class RepositoryBase
    {
        /// <summary>
        /// Executes the operation wrapped in a transaction that is automatically committed if no errors occur
        /// </summary>
        /// <typeparam name="TQueryResult">The type of the t query result.</typeparam>
        /// <param name="executeFunc">The execute function.</param>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>A Task&lt;TQueryResult&gt;.</returns>
        /// <remarks>
        /// .Net core has not yet implemented System.Transactions yet... but soon:
        /// https://github.com/dotnet/corefx/tree/master/src/System.Transactions
        /// </remarks>
        protected virtual async Task<TQueryResult> ExecuteAsTransaction<TQueryResult>(
            [NotNull] Func<DbConnection, DbTransaction, Task<TQueryResult>> executeFunc,
            [CanBeNull] IsolationLevel? isolationLevel = null)
        {
            if (executeFunc == null)
            {
                throw new ArgumentNullException(nameof(executeFunc));
            }

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
