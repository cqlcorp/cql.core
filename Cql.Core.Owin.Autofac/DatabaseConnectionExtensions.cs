// ***********************************************************************
// Assembly         : Cql.Core.Owin.Autofac
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="DatabaseConnectionExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Owin.Autofac
{
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Reflection;

    using global::Autofac;

    using Cql.Core.SqlServer;

    using JetBrains.Annotations;

    /// <summary>
    /// Class DatabaseConnectionExtensions.
    /// </summary>
    public static class DatabaseConnectionExtensions
    {
        /// <summary>
        /// Registers the database connection.
        /// </summary>
        /// <typeparam name="TDatabaseConnection">The type of database connection.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="connectionName">The connection string key for the connectionStrings config collection..</param>
        /// <param name="webAssembly">If provided, sets the ApplicationName property of the connection string to the AssemblyName of the specified assembly.</param>
        public static void RegisterDatabaseConnection<TDatabaseConnection>([NotNull] this ContainerBuilder builder, [NotNull] string connectionName, [CanBeNull] Assembly webAssembly = null)
            where TDatabaseConnection : DatabaseConnection, new()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

            if (webAssembly != null)
            {
                // Sets the Application Name property of the connection string to the web assembly name.
                var connBuilder = new SqlConnectionStringBuilder(connectionString) { ApplicationName = webAssembly.FullName };

                connectionString = connBuilder.ToString();
            }

            builder.Register(c => new TDatabaseConnection { ConnectionName = connectionName, ConnectionString = connectionString });
        }
    }
}
