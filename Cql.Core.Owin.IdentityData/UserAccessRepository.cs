namespace Cql.Core.Owin.IdentityData
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Cql.Core.Owin.Identity.Repositories;
    using Cql.Core.SqlServer;

    using Dapper;

    [SuppressMessage("ReSharper", "RedundantVerbatimPrefix")]
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public class UserAccessRepository : RepositoryBase, IUserAccessRepository
    {
        public UserAccessRepository(DatabaseConnection connection)
            : base(connection)
        {
        }

        public virtual Task GrantAccess(int userId)
        {
            var args = new { @Id = userId };

            const string sql = "UPDATE dbo.AspNetUsers " + "SET " + " AccessRevokedDate = null, " + " AccessRevokedBy = null " + "WHERE Id = @Id";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }

        public virtual Task RevokeAccess(int userId, int revokedByUserId)
        {
            var args = new { @Id = userId, @AccessRevokedBy = revokedByUserId };

            const string sql = "UPDATE dbo.AspNetUsers " + "SET " + " AccessRevokedDate = SYSDATETIMEOFFSET(), " + " AccessRevokedBy = @AccessRevokedBy " + "WHERE Id = @Id";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }
    }
}
