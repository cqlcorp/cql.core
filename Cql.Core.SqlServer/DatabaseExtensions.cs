namespace Cql.Core.SqlServer
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using StackExchange.Profiling.Data;

    public static class DatabaseExtensions
    {
        public static SqlCommand AsSqlCommand(this IDbCommand command)
        {
#if PROFILER
            var profiledDbCommand = command as ProfiledDbCommand;

            if (profiledDbCommand != null)
            {
                command = profiledDbCommand.InternalCommand;
            }

#endif
            return command as SqlCommand;
        }

        public static SqlConnection AsSqlConnection(this IDbConnection connection)
        {
#if PROFILER
            var profiledConnection = connection as ProfiledDbConnection;

            if (profiledConnection != null)
            {
                connection = profiledConnection.InnerConnection;
            }

#endif
            return connection as SqlConnection;
        }

        public static SqlParameterCollection GetParameters(this IDbCommand command)
        {
            return command.Parameters as SqlParameterCollection;
        }

        public static async Task OpenAsync(this IDbConnection connection)
        {
            var sqlConnection = connection.AsSqlConnection();

            if (sqlConnection != null)
            {
                await sqlConnection.OpenAsync();
                return;
            }

            connection.Open();
        }
    }
}
