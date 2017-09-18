// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="BulkcopyOperation.cs" company="CQL;Jeremy Bell">
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

    using JetBrains.Annotations;

    /// <summary>
    /// SQL Bulkcopy Operation options.
    /// </summary>
    /// <typeparam name="T">The strongly typed record</typeparam>
    public class BulkcopyOperation<T>
    {
        /// <summary>
        /// The columns
        /// </summary>
        [CanBeNull]
        private string[] columns;

        /// <summary>
        /// The destination table name
        /// </summary>
        [CanBeNull]
        private string destinationTableName;

        /// <summary>
        /// The records.
        /// </summary>
        [CanBeNull]
        private IEnumerable<T> records;

        /// <summary>
        /// Gets or sets the size of the batch.
        /// </summary>
        /// <value>The size of the batch.</value>
        public int? BatchSize { get; set; }

        /// <summary>
        /// Gets or sets the cancellation token.
        /// </summary>
        /// <value>The cancellation token.</value>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        [NotNull]
        public string[] Columns
        {
            get => LazyInitializer.EnsureInitialized(ref this.columns, TypeUtils.GetColumnNames<T>);
            set => this.columns = value;
        }

        /// <summary>
        /// Gets or sets the name of the destination table.
        /// </summary>
        /// <value>The name of the destination table.</value>
        [NotNull]
        public string DestinationTableName
        {
            get => LazyInitializer.EnsureInitialized(ref this.destinationTableName, TypeUtils.GetTableName<T>);
            set => this.destinationTableName = value;
        }

        /// <summary>
        /// Gets or sets the records.
        /// </summary>
        /// <value>The records.</value>
        [NotNull]
        public IEnumerable<T> Records
        {
            get => this.records ?? throw new InvalidOperationException($"The {nameof(this.Records)} property has not been initialized.");
            set => this.records = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the SQL bulk copy options.
        /// </summary>
        /// <value>The SQL bulk copy options.</value>
        public SqlBulkCopyOptions? SqlBulkCopyOptions { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>The timeout.</value>
        public TimeSpan? Timeout { get; set; }
    }
}
