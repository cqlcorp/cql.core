using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

#if PROFILER
using StackExchange.Profiling.Data;
#endif

namespace Cql.Core.SqlServer
{
    [SuppressMessage("ReSharper", "RedundantVerbatimPrefix")]
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public abstract partial class RepositoryBase : IDbConnectionCreator
    {
        private readonly DatabaseConnection _connection;
#if PROFILER
        private readonly MiniProfiler.Integrations.CustomDbProfiler _profiler;
#endif
        protected EventHandler<CommandsExecutedEventArgs> OnCommandsExecuted;
        protected EventHandler<ExecuteErrorEventArgs> OnExecuteError;


        protected RepositoryBase(DatabaseConnection connection)
        {
            _connection = connection;
#if PROFILER
         _profiler = new MiniProfiler.Integrations.CustomDbProfiler();
#endif
        }

        protected string ConnectionString => _connection.ConnectionString;

        DbConnection IDbConnectionCreator.CreateDbConnection()
        {
            return CreateConnection();
        }

        void IDbConnectionCreator.RaiseCommandsExecutedEvent()
        {
            RaiseCommandsExecuted();
        }

        void IDbConnectionCreator.RaiseExecuteErrorEvent(Exception ex)
        {
            RaiseExecuteError(ex);
        }

        protected virtual DbConnection CreateConnection()
        {
            return CreateConnection(ConnectionString);
        }

        protected virtual DbConnection CreateConnection(string connectionString)
        {
#if PROFILER
            return new ProfiledDbConnection(new SqlConnection(connectionString), _profiler);
#else
            return new SqlConnection(connectionString);
#endif
        }

        protected virtual TQueryResult Execute<TQueryResult>(
            Func<DbConnection, TQueryResult> executeFunc)
        {
            using (var db = CreateConnection())
            {
                return executeFunc(db);
            }
        }

        protected virtual async Task<TQueryResult> Execute<TQueryResult>(
            Func<DbConnection, Task<TQueryResult>> executeFunc)
        {
            using (var db = CreateConnection())
            {
                return await executeFunc(db).ConfigureAwait(false);
            }
        }

        private void RaiseCommandsExecuted()
        {
#if PROFILER
            OnCommandsExecuted?.Invoke(this, new CommandsExecutedEventArgs(_profiler));
#else
            OnCommandsExecuted?.Invoke(this, new CommandsExecutedEventArgs());
#endif
        }

        private void RaiseExecuteError(Exception ex)
        {
            OnExecuteError?.Invoke(this, new ExecuteErrorEventArgs(ex));
        }
    }
}
