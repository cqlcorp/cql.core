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
    public class RoleRepository : RepositoryBase, IRoleRepository
    {
        public RoleRepository(DatabaseConnection connection)
            : base(connection)
        {
        }

        public virtual Task CreateAsync(IdentityRole role)
        {
            var args = new { role.Id, role.Name };

            const string sql = "INSERT INTO dbo.AspNetRoles (Id, Name) " + "VALUES (@Id, @Name)";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }

        public virtual Task DeleteAsync(IdentityRole role)
        {
            var args = new { @Id = role.Id };

            const string sql = "DELETE FROM dbo.AspNetRoles WHERE Id = @Id";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }

        public virtual Task<IdentityRole> FindByIdAsync(int roleId)
        {
            var args = new { @Id = roleId };

            const string sql = "SELECT Id, Name " + "FROM dbo.AspNetRoles " + "WHERE Id = @Id";

            return this.Execute(db => db.QueryFirstOrDefaultAsync<IdentityRole>(sql, args));
        }

        public virtual Task<IdentityRole> FindByNameAsync(string roleName)
        {
            var args = new { @Name = roleName };

            const string sql = "SELECT Id, Name " + "FROM dbo.AspNetRoles " + "WHERE Name = @Name";

            return this.Execute(db => db.QueryFirstOrDefaultAsync<IdentityRole>(sql, args));
        }

        public virtual Task UpdateAsync(IdentityRole role)
        {
            var args = new { @Id = role.Id, @Name = role.Name };

            const string sql = "UPDATE dbo.AspNetRoles" + "SET" + " Name = @Name " + "WHERE Id = @Id";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }
    }
}
