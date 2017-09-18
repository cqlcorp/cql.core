// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="RepositoryBase.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

#if PROFILER
using StackExchange.Profiling.Data;
using MiniProfiler.Integrations;
#endif

namespace Cql.Core.SqlServer
{
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

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

#if PROFILER
        private readonly CustomDbProfiler _profiler;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        protected RepositoryBase([NotNull] DatabaseConnection connection)
        {
            Contract.Requires(connection != null);

            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
#if PROFILER
            this._profiler = new CustomDbProfiler();
#endif
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
        protected string ConnectionString => this.connection.ConnectionString;

        /// <summary>
        /// Creates the database connection.
        /// </summary>
        /// <returns><see cref="DbConnection"/></returns>
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
        /// <returns><see cref="DbConnection"/></returns>
        [NotNull]
        protected virtual DbConnection CreateConnection()
        {
            return this.CreateConnection(this.ConnectionString);
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns><see cref="DbConnection"/></returns>
        [NotNull]
        protected virtual DbConnection CreateConnection([NotNull] string connectionString)
        {
            Contract.Requires(!string.IsNullOrEmpty(connectionString));

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("message", nameof(connectionString));
            }

#if PROFILER
            return new ProfiledDbConnection(new SqlConnection(connectionString), this._profiler);
#else
            return new SqlConnection(connectionString);
#endif
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
                return executeFunc(db);
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
                return await executeFunc(db).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Raises the commands executed.
        /// </summary>
        private void RaiseCommandsExecuted()
        {
#if PROFILER
            this.OnCommandsExecuted?.Invoke(this, new CommandsExecutedEventArgs(this._profiler));
#else
            this.OnCommandsExecuted?.Invoke(this, new CommandsExecutedEventArgs());
#endif
        }

        /// <summary>
        /// Raises the execute error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void RaiseExecuteError([NotNull] Exception ex)
        {
            this.OnExecuteError?.Invoke(this, new ExecuteErrorEventArgs(ex));
        }
    }
}
