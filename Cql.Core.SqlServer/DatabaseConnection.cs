namespace Cql.Core.SqlServer
{
    using System.Data.Common;
    using System.Data.SqlClient;

    public class DatabaseConnection
    {
        public string ConnectionName { get; set; }

        public string ConnectionString { get; set; }

        public DbProviderFactory DbProviderFactory { get; set; } = SqlClientFactory.Instance;
    }
}
