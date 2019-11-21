// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-16-2017
// ***********************************************************************
// <copyright file="DatabaseExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Cql.Core.SqlServer
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    using JetBrains.Annotations;
#if PROFILER
    using StackExchange.Profiling.Data;
#endif

    /// <summary>
    /// Class DatabaseExtensions.
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Returns the <paramref name="command"/> as a <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns><see cref="SqlCommand"/>.</returns>
        [CanBeNull]
        public static SqlCommand AsSqlCommand([NotNull] this IDbCommand command)
        {
            Contract.Requires(command != null);
#if PROFILER
            if (command is ProfiledDbCommand profiledDbCommand)
            {
                command = profiledDbCommand.InternalCommand;
            }
#endif
            return command as SqlCommand;
        }

        /// <summary>
        /// Returns the <paramref name="connection"/> as a <see cref="SqlConnection"/>.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <exception cref="InvalidCastException">Thrown if the underlying connection type is not a SqlConnection</exception>
        /// <returns><see cref="SqlConnection"/>.</returns>
        [CanBeNull]
        public static SqlConnection AsSqlConnection([NotNull] this IDbConnection connection)
        {
            Contract.Requires(connection != null);
#if PROFILER
            if (connection is ProfiledDbConnection profiledConnection)
            {
                connection = profiledConnection.InnerConnection;
            }
#endif
            return connection as SqlConnection;
        }
		
        [ItemNotNull]
        [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "This is a general purpose method")]
        public static async Task<IDbCommand> GetOpenCommandAsync(this IDbConnection connection, string commandText, CommandType commandType, CancellationToken cancellationToken = default)
        {
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync(cancellationToken);
            }

            var command = connection.CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;

            return command;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns><see cref="SqlParameterCollection"/>.</returns>
        [CanBeNull]
        public static SqlParameterCollection GetParameters([NotNull] this IDbCommand command)
        {
            Contract.Requires(command != null);

            return command.Parameters as SqlParameterCollection;
        }

        /// <summary>
        /// Attempts to use the OpenAsync method of the connection if supported, otherwise the synchronous Open method is invoked.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An awaitable task.</returns>
        public static async Task OpenAsync([NotNull] this IDbConnection connection, CancellationToken cancellationToken = default)
        {
            Contract.Requires(connection != null);

            var sqlConnection = connection.AsSqlConnection();

            if (sqlConnection != null)
            {
                await sqlConnection.OpenAsync(cancellationToken);
                return;
            }

            connection.Open();
        }
    }
}
