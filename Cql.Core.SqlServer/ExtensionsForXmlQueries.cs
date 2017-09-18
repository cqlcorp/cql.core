// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="ExtensionsForXmlQueries.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Collections.Concurrent;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    using JetBrains.Annotations;

    /// <summary>
    /// Class ExtensionsForXmlQueries.
    /// </summary>
    public static class ExtensionsForXmlQueries
    {
        /// <summary>
        /// The serializer cache
        /// </summary>
        [NotNull]
        private static readonly ConcurrentDictionary<Type, XmlSerializer> SerializerCache = new ConcurrentDictionary<Type, XmlSerializer>();

        /// <summary>
        /// Executes the current <paramref name="command"/> and returns the deserialized result.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>An awaitable task of <typeparamref name="TResult"/>.</returns>
        /// <exception cref="InvalidOperationException">ExecuteXmlReaderAsync method not supported.</exception>
        [ItemCanBeNull]
        public static async Task<TResult> ExecuteXmlResultAsync<TResult>([NotNull] this IDbCommand command)
        {
            var sqlCommand = command.AsSqlCommand();

            if (sqlCommand == null)
            {
                throw new InvalidOperationException("ExecuteXmlReaderAsync method not supported.");
            }

            return await sqlCommand.ExecuteXmlResultAsync<TResult>();
        }

        /// <summary>
        /// Executes the current <paramref name="sqlCommand"/> and returns the deserialized result.
        /// </summary>
        /// <typeparam name="TResult">The strongly typed result.</typeparam>
        /// <param name="sqlCommand">The SQL command.</param>
        /// <returns>An awaitable task of <typeparamref name="TResult"/>.</returns>
        [ItemCanBeNull]
        public static async Task<TResult> ExecuteXmlResultAsync<TResult>([NotNull] this SqlCommand sqlCommand)
        {
            var type = typeof(TResult);

            var xmlSerializer = SerializerCache.GetOrAdd(type, t => new XmlSerializer(t));

            using (var r = await sqlCommand.ExecuteXmlReaderAsync().ConfigureAwait(false))
            {
                r.MoveToContent();

                if (!r.EOF && xmlSerializer.CanDeserialize(r))
                {
                    return (TResult)xmlSerializer.Deserialize(r);
                }
            }

            return default(TResult);
        }
    }
}
