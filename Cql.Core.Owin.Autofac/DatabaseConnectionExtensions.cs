using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;

using Autofac;

using Cql.Core.SqlServer;

namespace Cql.Core.Owin.Autofac
{
    public static class DatabaseConnectionExtensions
    {
        public static void RegisterDatabaseConnection<TDatabaseConnection>(this ContainerBuilder builder, string connectionName, Assembly webAssembly = null)
            where TDatabaseConnection : DatabaseConnection, new()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

            if (webAssembly != null)
            {
                // Sets the Application Name property of the connection string to the web assembly name.
                var connBuilder = new SqlConnectionStringBuilder(connectionString)
                {
                    ApplicationName = webAssembly.FullName
                };

                connectionString = connBuilder.ToString();
            }

            builder.Register(
                c => new TDatabaseConnection
                {
                    ConnectionName = connectionName,
                    ConnectionString = connectionString
                });
        }
    }
}
