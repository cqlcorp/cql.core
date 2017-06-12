using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using FastMember;

namespace Cql.Core.SqlServer
{
    public abstract partial class RepositoryBase
    {
        protected Task ExecuteBulkInsertAsync<T>(IEnumerable<T> values)
        {
            return ExecuteBulkInsertAsync(new BulkcopyOperation<T>
            {
                Records = values
            });
        }

        protected async Task ExecuteBulkInsertAsync<T>(BulkcopyOperation<T> bulkcopyOperation)
        {
            using (var sqlBulkCopy = new SqlBulkCopy(ConnectionString, bulkcopyOperation.SqlBulkCopyOptions.GetValueOrDefault(SqlBulkCopyOptions.Default)))
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
