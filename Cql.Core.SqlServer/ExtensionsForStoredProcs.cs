namespace Cql.Core.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Cql.Core.Common.Types;

    using Dapper;

    public static class ExtensionsForStoredProcs
    {
        public static Task<int> SpExecNonQueryAsync(
            this IDbConnection db,
            string storedProcName,
            object args = null,
            IDbTransaction transaction = null,
            TimeSpan? commandTimeout = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return db.ExecuteAsync(CreateCommandArgs(storedProcName, args, transaction, commandTimeout, cancellationToken));
        }

        public static async Task<PagedResult<T>> SpExecPagedResult<T>(
            this IDbConnection db,
            string storedProcName,
            DynamicParameters args,
            IDbTransaction transaction = null,
            TimeSpan? commandTimeout = null,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : ISearchResult
        {
            var searchResults = await db.QueryAsync<T>(CreateCommandArgs(storedProcName, args, transaction, commandTimeout, cancellationToken));

            var results = searchResults?.ToList() ?? new List<T>();

            var totalRecords = results.GetTotalRecords();

            return new PagedResult<T>(args.GetPagingInfo()) { Results = results, TotalRecords = totalRecords };
        }

        public static Task<IEnumerable<T>> SpExecQueryAsync<T>(
            this IDbConnection db,
            string storedProcName,
            object args = null,
            IDbTransaction transaction = null,
            TimeSpan? commandTimeout = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return db.QueryAsync<T>(CreateCommandArgs(storedProcName, args, transaction, commandTimeout, cancellationToken));
        }

        public static Task<SqlMapper.GridReader> SpExecQueryMultipleAsync(
            this IDbConnection db,
            string storedProcName,
            object args = null,
            IDbTransaction transaction = null,
            TimeSpan? commandTimeout = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return db.QueryMultipleAsync(CreateCommandArgs(storedProcName, args, transaction, commandTimeout, cancellationToken));
        }

        public static Task<T> SpExecScalarAsync<T>(
            this IDbConnection db,
            string storedProcName,
            object args = null,
            IDbTransaction transaction = null,
            TimeSpan? commandTimeout = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return db.ExecuteScalarAsync<T>(CreateCommandArgs(storedProcName, args, transaction, commandTimeout, cancellationToken));
        }

        public static Task<T> SpExecSingleAsync<T>(this IDbConnection db, string storedProcName, object args = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return db.QuerySingleAsync<T>(storedProcName, args, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);
        }

        public static Task<T> SpExecSingleOrDefaultAsync<T>(
            this IDbConnection db,
            string storedProcName,
            object args = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null)
        {
            return db.QuerySingleOrDefaultAsync<T>(storedProcName, args, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout);
        }

        private static CommandDefinition CreateCommandArgs(
            string storedProcName,
            object args,
            IDbTransaction transaction,
            TimeSpan? commandTimeout,
            CancellationToken cancellationToken)
        {
            var timeoutSeconds = default(int?);

            if (commandTimeout.HasValue)
            {
                timeoutSeconds = Convert.ToInt32(commandTimeout.Value.TotalSeconds);
            }

            return new CommandDefinition(storedProcName, args, transaction, timeoutSeconds, CommandType.StoredProcedure, cancellationToken: cancellationToken);
        }
    }
}
