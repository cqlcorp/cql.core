// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 12-20-2017
// ***********************************************************************
// <copyright file="RepositoryBase.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using StackExchange.Profiling;
    using StackExchange.Profiling.Data;

    /// <summary>
    /// Class RepositoryBase.
    /// </summary>
    /// <seealso cref="Cql.Core.SqlServer.IDbConnectionCreator" />
    public abstract partial class RepositoryBase : IDbConnectionCreator
    {
        /// <summary>
        /// The connection
        /// </summary>
        [NotNull]
        private readonly DatabaseConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        protected RepositoryBase([NotNull] DatabaseConnection connection)
        {
            Contract.Requires(connection != null);

            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Gets or sets the on commands executed event.
        /// </summary>
        /// <value>The on commands executed.</value>
        [CanBeNull]
        public EventHandler<CommandsExecutedEventArgs> OnCommandsExecuted { get; set; }

        /// <summary>
        /// Gets or sets the on execute error event.
        /// </summary>
        /// <value>The on execute error.</value>
        [CanBeNull]
        public EventHandler<ExecuteErrorEventArgs> OnExecuteError { get; set; }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        [NotNull]
        protected string ConnectionString => this.connection.ConnectionString ?? throw new InvalidOperationException("The connection string cannot be null or empty.");

        /// <summary>
        /// Creates the database connection.
        /// </summary>
        /// <returns>
        ///     <see cref="DbConnection" />
        /// </returns>
        DbConnection IDbConnectionCreator.CreateDbConnection()
        {
            return this.CreateConnection();
        }

        /// <summary>
        /// Raises the commands executed event.
        /// </summary>
        void IDbConnectionCreator.RaiseCommandsExecutedEvent()
        {
            this.RaiseCommandsExecuted();
        }

        /// <summary>
        /// Raises the execute error event.
        /// </summary>
        /// <param name="ex">The ex.</param>
        void IDbConnectionCreator.RaiseExecuteErrorEvent(Exception ex)
        {
            this.RaiseExecuteError(ex);
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>
        ///     <see cref="DbConnection" />
        /// </returns>
        [NotNull]
        protected virtual DbConnection CreateConnection()
        {
            return this.CreateConnection(this.ConnectionString);
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        ///     <see cref="DbConnection" />
        /// </returns>
        [NotNull]
        protected virtual DbConnection CreateConnection([NotNull] string connectionString)
        {
            Contract.Requires(!string.IsNullOrEmpty(connectionString));

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("The connection string cannot be null or empty", nameof(connectionString));
            }

            return new ProfiledDbConnection(new SqlConnection(connectionString), MiniProfiler.Current);
        }

        /// <summary>
        /// Executes the specified execute function.
        /// </summary>
        /// <typeparam name="TQueryResult">The type of the query result.</typeparam>
        /// <param name="executeFunc">The execute function.</param>
        /// <returns>The query result.</returns>
        protected virtual TQueryResult Execute<TQueryResult>([NotNull] Func<DbConnection, TQueryResult> executeFunc)
        {
            Contract.Requires(executeFunc != null);

            if (executeFunc == null)
            {
                throw new ArgumentNullException(nameof(executeFunc));
            }

            using (var db = this.CreateConnection())
            {
                try
                {
                    var result = executeFunc(db);
                    this.RaiseCommandsExecuted();
                    return result;
                }
                catch (Exception ex)
                {
                    this.RaiseExecuteError(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Executes the specified execute function.
        /// </summary>
        /// <typeparam name="TQueryResult">The type of the t query result.</typeparam>
        /// <param name="executeFunc">The execute function.</param>
        /// <returns>An awaitable task of the query result.</returns>
        protected virtual async Task<TQueryResult> Execute<TQueryResult>([NotNull] Func<DbConnection, Task<TQueryResult>> executeFunc)
        {
            Contract.Requires(executeFunc != null);

            if (executeFunc == null)
            {
                throw new ArgumentNullException(nameof(executeFunc));
            }

            using (var db = this.CreateConnection())
            {
                try
                {
                    var result = await executeFunc(db).ConfigureAwait(false);
                    this.RaiseCommandsExecuted();
                    return result;
                }
                catch (Exception ex)
                {
                    this.RaiseExecuteError(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Raises the commands executed.
        /// </summary>
        private void RaiseCommandsExecuted()
        {
            this.OnCommandsExecuted?.Invoke(this, new CommandsExecutedEventArgs(MiniProfiler.Current));
        }

        /// <summary>
        /// Raises the execute error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void RaiseExecuteError([NotNull] Exception ex)
        {
            this.OnExecuteError?.Invoke(this, new ExecuteErrorEventArgs(MiniProfiler.Current, ex));
        }
    }
}
