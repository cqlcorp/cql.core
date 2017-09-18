// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="DatabaseConnection.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading;

    using JetBrains.Annotations;

    /// <summary>
    /// Class DatabaseConnection.
    /// </summary>
    public class DatabaseConnection
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        [CanBeNull]
        private DbProviderFactory providerFactory;

        /// <summary>
        /// Gets or sets the name of the connection.
        /// </summary>
        /// <value>The name of the connection.</value>
        [CanBeNull]
        public string ConnectionName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        [CanBeNull]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the database provider factory.
        /// </summary>
        /// <value>The database provider factory.</value>
        [NotNull]
        public DbProviderFactory DbProviderFactory
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref this.providerFactory, () => SqlClientFactory.Instance);
            }

            set => this.providerFactory = value;
        }
    }
}
