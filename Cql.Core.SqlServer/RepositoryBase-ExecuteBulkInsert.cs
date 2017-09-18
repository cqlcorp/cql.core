// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="RepositoryBase-ExecuteBulkInsert.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    using FastMember;

    using JetBrains.Annotations;

    /// <summary>
    /// Class RepositoryBase.
    /// </summary>
    /// <seealso cref="Cql.Core.SqlServer.IDbConnectionCreator" />
    public abstract partial class RepositoryBase
    {
        /// <summary>
        /// Executes a bulkcopy insert using the default conventions.
        /// </summary>
        /// <typeparam name="T">The type of record to insert</typeparam>
        /// <param name="values">The values.</param>
        /// <returns>An awaitable task.</returns>
        [NotNull]
        protected Task ExecuteBulkInsertAsync<T>([NotNull] IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return this.ExecuteBulkInsertAsync(new BulkcopyOperation<T> { Records = values });
        }

        /// <summary>
        /// Executes a SQL bulkcopy operation using the specified <paramref name="bulkcopyOperation" /> options.
        /// </summary>
        /// <typeparam name="T">The type of record to insert.</typeparam>
        /// <param name="bulkcopyOperation">The bulkcopy operation options.</param>
        /// <returns>An awaitable task.</returns>
        protected async Task ExecuteBulkInsertAsync<T>([NotNull] BulkcopyOperation<T> bulkcopyOperation)
        {
            if (bulkcopyOperation == null)
            {
                throw new ArgumentNullException(nameof(bulkcopyOperation));
            }

            using (var sqlBulkCopy = new SqlBulkCopy(this.ConnectionString, bulkcopyOperation.SqlBulkCopyOptions.GetValueOrDefault(SqlBulkCopyOptions.Default)))
            {
                sqlBulkCopy.DestinationTableName = bulkcopyOperation.DestinationTableName;

                sqlBulkCopy.BatchSize = bulkcopyOperation.BatchSize.GetValueOrDefault(5000);

                if (bulkcopyOperation.Timeout.HasValue)
                {
                    sqlBulkCopy.BulkCopyTimeout = bulkcopyOperation.Timeout.Value == Timeout.InfiniteTimeSpan ? 0 : Convert.ToInt32(bulkcopyOperation.Timeout.Value.TotalSeconds);
                }

                foreach (var column in bulkcopyOperation.Columns)
                {
                    sqlBulkCopy.ColumnMappings.Add(column, column);
                }

                using (var reader = ObjectReader.Create(bulkcopyOperation.Records, bulkcopyOperation.Columns))
                {
                    await sqlBulkCopy.WriteToServerAsync(reader, bulkcopyOperation.CancellationToken);
                }
            }
        }
    }
}
