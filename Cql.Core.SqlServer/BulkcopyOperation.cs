namespace Cql.Core.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading;

    public class BulkcopyOperation<T>
    {
        private string[] _columns;

        private string _destinationTableName;

        public int? BatchSize { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public string[] Columns
        {
            get => LazyInitializer.EnsureInitialized(ref this._columns, TypeUtils.GetColumnNames<T>);
            set => this._columns = value;
        }

        public string DestinationTableName
        {
            get => LazyInitializer.EnsureInitialized(ref this._destinationTableName, TypeUtils.GetTableName<T>);
            set => this._destinationTableName = value;
        }

        public IEnumerable<T> Records { get; set; }

        public SqlBulkCopyOptions? SqlBulkCopyOptions { get; set; }

        public TimeSpan? Timeout { get; set; }
    }
}
