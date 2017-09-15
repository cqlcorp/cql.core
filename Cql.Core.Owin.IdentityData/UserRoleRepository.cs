namespace Cql.Core.Owin.IdentityData
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using Cql.Core.Owin.Identity.Repositories;
    using Cql.Core.Owin.Identity.Types;
    using Cql.Core.SqlServer;

    using Dapper;

    [SuppressMessage("ReSharper", "RedundantVerbatimPrefix")]
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public class UserRoleRepository : RepositoryBase, IUserRoleRepository
    {
        public UserRoleRepository(DatabaseConnection connection)
            : base(connection)
        {
        }

        public virtual Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            var args = new { @UserId = user.Id, @RoleName = roleName };

            return this.Execute(db => db.ExecuteAsync("dbo.spUserRoles_add", args, commandType: CommandType.StoredProcedure));
        }

        public virtual async Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            var args = new { @UserId = user.Id };

            const string sql = "SELECT r.Name " + "FROM dbo.AspNetRoles r " + "JOIN dbo.AspNetUserRoles u ON u.RoleId = r.Id " + "WHERE u.UserId = @UserId";

            var q = await this.Execute(db => db.QueryAsync<string>(sql, args));

            return q.ToList();
        }

        public virtual Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            var args = new { @UserId = user.Id, @RoleName = roleName };

            return this.Execute(db => db.ExecuteScalarAsync<bool>("dbo.spUserRoles_exists", args, commandType: CommandType.StoredProcedure));
        }

        public virtual Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            var args = new { @UserId = user.Id, @RoleName = roleName };

            return this.Execute(db => db.ExecuteAsync("dbo.spUserRoles_remove", args, commandType: CommandType.StoredProcedure));
        }
    }
}
