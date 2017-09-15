namespace Cql.Core.Owin.IdentityData
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Cql.Core.Owin.Identity.Repositories;
    using Cql.Core.Owin.Identity.Types;
    using Cql.Core.SqlServer;

    using Dapper;

    [SuppressMessage("ReSharper", "RedundantVerbatimPrefix")]
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public class ClientRepository : RepositoryBase, IClientRepository
    {
        public ClientRepository(DatabaseConnection connection)
            : base(connection)
        {
        }

        public virtual Task<Client> FindClientByIdAsync(string clientId)
        {
            var args = new { @ClientId = clientId };

            const string sql = "SELECT * " + "FROM dbo.Clients " + "WHERE Id = @ClientId";

            return this.Execute(db => db.QuerySingleOrDefaultAsync<Client>(sql, args));
        }
    }
}
