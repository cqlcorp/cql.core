#if PROFILER
using StackExchange.Profiling.Data;

#endif

namespace Cql.Core.SqlServer
{
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using MiniProfiler.Integrations;

    [SuppressMessage("ReSharper", "RedundantVerbatimPrefix")]
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public abstract partial class RepositoryBase : IDbConnectionCreator
    {
        protected EventHandler<CommandsExecutedEventArgs> OnCommandsExecuted;

        protected EventHandler<ExecuteErrorEventArgs> OnExecuteError;

        private readonly DatabaseConnection _connection;
#if PROFILER
        private readonly CustomDbProfiler _profiler;
#endif

        protected RepositoryBase(DatabaseConnection connection)
        {
            this._connection = connection;
#if PROFILER
            this._profiler = new CustomDbProfiler();
#endif
        }

        protected string ConnectionString => this._connection.ConnectionString;

        DbConnection IDbConnectionCreator.CreateDbConnection()
        {
            return this.CreateConnection();
        }

        void IDbConnectionCreator.RaiseCommandsExecutedEvent()
        {
            this.RaiseCommandsExecuted();
        }

        void IDbConnectionCreator.RaiseExecuteErrorEvent(Exception ex)
        {
            this.RaiseExecuteError(ex);
        }

        protected virtual DbConnection CreateConnection()
        {
            return this.CreateConnection(this.ConnectionString);
        }

        protected virtual DbConnection CreateConnection(string connectionString)
        {
#if PROFILER
            return new ProfiledDbConnection(new SqlConnection(connectionString), this._profiler);
#else
            return new SqlConnection(connectionString);
#endif
        }

        protected virtual TQueryResult Execute<TQueryResult>(Func<DbConnection, TQueryResult> executeFunc)
        {
            using (var db = this.CreateConnection())
            {
                return executeFunc(db);
            }
        }

        protected virtual async Task<TQueryResult> Execute<TQueryResult>(Func<DbConnection, Task<TQueryResult>> executeFunc)
        {
            using (var db = this.CreateConnection())
            {
                return await executeFunc(db).ConfigureAwait(false);
            }
        }

        private void RaiseCommandsExecuted()
        {
#if PROFILER
            this.OnCommandsExecuted?.Invoke(this, new CommandsExecutedEventArgs(this._profiler));
#else
            OnCommandsExecuted?.Invoke(this, new CommandsExecutedEventArgs());
#endif
        }

        private void RaiseExecuteError(Exception ex)
        {
            this.OnExecuteError?.Invoke(this, new ExecuteErrorEventArgs(ex));
        }
    }
}
