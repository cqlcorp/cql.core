// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="ExtensionsForStoredProcs.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Cql.Core.Common.Types;

    using Dapper;

    using JetBrains.Annotations;

    /// <summary>
    /// Class ExtensionsForStoredProcs.
    /// </summary>
    public static class ExtensionsForStoredProcs
    {
        /// <summary>
        /// Executes a stored procedure that does not return any data rows.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="storedProcName">Name of the stored proc.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable task</returns>
        public static async Task<int> SpExecNonQueryAsync(
            [NotNull] this IDbConnection db,
            [NotNull] string storedProcName,
            [CanBeNull] object args = null,
            [CanBeNull] IDbTransaction transaction = null,
            TimeSpan? commandTimeout = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.Requires(db != null);
            Contract.Requires(!string.IsNullOrEmpty(storedProcName));

            return await db.ExecuteAsync(CreateCommandArgs(storedProcName, args, transaction, commandTimeout, cancellationToken)).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes a stored procedure with paging support.
        /// </summary>
        /// <typeparam name="T">The type of result record.</typeparam>
        /// <param name="db">The database.</param>
        /// <param name="storedProcName">Name of the stored proc.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="PagedResult{T}" />.</returns>
        public static async Task<PagedResult<T>> SpExecPagedResult<T>(
            [NotNull] this IDbConnection db,
            [NotNull] string storedProcName,
            [NotNull] DynamicParameters args,
            [CanBeNull] IDbTransaction transaction = null,
            TimeSpan? commandTimeout = null,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : ISearchResult
        {
            Contract.Requires(db != null);
            Contract.Requires(!string.IsNullOrEmpty(storedProcName));

            var searchResults = await db.QueryAsync<T>(CreateCommandArgs(storedProcName, args, transaction, commandTimeout, cancellationToken)).ConfigureAwait(false);

            var results = searchResults?.ToList() ?? new List<T>();

            var totalRecords = results.GetTotalRecords();

            return new PagedResult<T>(args.GetPagingInfo()) { Results = results, TotalRecords = totalRecords };
        }

        /// <summary>
        /// Executes a stored procedure that returns multiple data rows.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="db">The database.</param>
        /// <param name="storedProcName">Name of the stored proc.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable <see cref="Task{TResult}" /></returns>
        public static async Task<IEnumerable<TResult>> SpExecQueryAsync<TResult>(
            [NotNull] this IDbConnection db,
            [NotNull] string storedProcName,
            [CanBeNull] object args = null,
            [CanBeNull] IDbTransaction transaction = null,
            TimeSpan? commandTimeout = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.Requires(db != null);
            Contract.Requires(!string.IsNullOrEmpty(storedProcName));

            return await db.QueryAsync<TResult>(CreateCommandArgs(storedProcName, args, transaction, commandTimeout, cancellationToken)).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes a stored procedure that returns multiple result sets.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="storedProcName">Name of the stored proc.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     <see cref="SqlMapper.GridReader" />
        /// </returns>
        [ItemNotNull]
        public static async Task<SqlMapper.GridReader> SpExecQueryMultipleAsync(
            [NotNull] this IDbConnection db,
            [NotNull] string storedProcName,
            [CanBeNull] object args = null,
            [CanBeNull] IDbTransaction transaction = null,
            TimeSpan? commandTimeout = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.Requires(db != null);
            Contract.Requires(!string.IsNullOrEmpty(storedProcName));

            return await db.QueryMultipleAsync(CreateCommandArgs(storedProcName, args, transaction, commandTimeout, cancellationToken)).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes a stored proc that only returns a single value.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="db">The database.</param>
        /// <param name="storedProcName">Name of the stored proc.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     <see cref="Task{TResult}" />
        /// </returns>
        [ItemCanBeNull]
        public static async Task<TResult> SpExecScalarAsync<TResult>(
            [NotNull] this IDbConnection db,
            [NotNull] string storedProcName,
            [CanBeNull] object args = null,
            [CanBeNull] IDbTransaction transaction = null,
            TimeSpan? commandTimeout = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Contract.Requires(db != null);
            Contract.Requires(!string.IsNullOrEmpty(storedProcName));

            return await db.ExecuteScalarAsync<TResult>(CreateCommandArgs(storedProcName, args, transaction, commandTimeout, cancellationToken)).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes a stored proc that only returns a single row.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="db">The database.</param>
        /// <param name="storedProcName">Name of the stored proc.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>An awaitable <see cref="Task{TResult}" /></returns>
        public static async Task<TResult> SpExecSingleAsync<TResult>(
            [NotNull] this IDbConnection db,
            [NotNull] string storedProcName,
            [CanBeNull] object args = null,
            [CanBeNull] IDbTransaction transaction = null,
            int? commandTimeout = null)
        {
            Contract.Requires(db != null);
            Contract.Requires(!string.IsNullOrEmpty(storedProcName));

            return await db.QuerySingleAsync<TResult>(storedProcName, args, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: commandTimeout)
                       .ConfigureAwait(false);
        }

        /// <summary>
        /// Executes a stored proc that only returns a single row.
        /// </summary>
        /// <typeparam name="TResult">The type of result.</typeparam>
        /// <param name="db">The database.</param>
        /// <param name="storedProcName">Name of the stored proc.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <returns>An awaitable <see cref="Task{TResult}" /></returns>
        [ItemCanBeNull]
        public static async Task<TResult> SpExecSingleOrDefaultAsync<TResult>(
            [NotNull] this IDbConnection db,
            [NotNull] string storedProcName,
            [CanBeNull] object args = null,
            [CanBeNull] IDbTransaction transaction = null,
            int? commandTimeout = null)
        {
            Contract.Requires(db != null);
            Contract.Requires(!string.IsNullOrEmpty(storedProcName));

            return await db.QuerySingleOrDefaultAsync<TResult>(
                       storedProcName,
                       args,
                       commandType: CommandType.StoredProcedure,
                       transaction: transaction,
                       commandTimeout: commandTimeout).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a <see cref="CommandDefinition" /> for use with a stored proc.
        /// </summary>
        /// <param name="storedProcName">Name of the stored proc.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     <see cref="CommandDefinition" />
        /// </returns>
        private static CommandDefinition CreateCommandArgs(
            [NotNull] string storedProcName,
            [CanBeNull] object args,
            [CanBeNull] IDbTransaction transaction,
            TimeSpan? commandTimeout,
            CancellationToken cancellationToken)
        {
            Contract.Requires(!string.IsNullOrEmpty(storedProcName));

            var timeoutSeconds = default(int?);

            if (commandTimeout.HasValue)
            {
                timeoutSeconds = Convert.ToInt32(commandTimeout.Value.TotalSeconds);
            }

            return new CommandDefinition(storedProcName, args, transaction, timeoutSeconds, CommandType.StoredProcedure, cancellationToken: cancellationToken);
        }
    }
}
