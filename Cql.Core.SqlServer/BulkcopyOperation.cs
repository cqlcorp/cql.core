using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace Cql.Core.SqlServer
{
    public class BulkcopyOperation<T>
    {
        private string[] _columns;
        private string _destinationTableName;

        public string DestinationTableName
        {
            get { return LazyInitializer.EnsureInitialized(ref _destinationTableName, TypeUtils.GetTableName<T>); }
            set { _destinationTableName = value; }
        }

        public string[] Columns
        {
            get { return LazyInitializer.EnsureInitialized(ref _columns, TypeUtils.GetColumnNames<T>); }
            set { _columns = value; }
        }

        public int? BatchSize { get; set; }

        public SqlBulkCopyOptions? SqlBulkCopyOptions { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public TimeSpan? Timeout { get; set; }

        public IEnumerable<T> Records { get; set; }
    }
}
