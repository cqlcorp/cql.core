namespace Cql.Core.Owin.IdentityData
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Cql.Core.Common.Extensions;
    using Cql.Core.Owin.Identity.Repositories;
    using Cql.Core.Owin.Identity.Types;
    using Cql.Core.SqlServer;

    using Dapper;

    using Microsoft.AspNet.Identity;

    [SuppressMessage("ReSharper", "RedundantVerbatimPrefix")]
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public class UserLoginRepository : RepositoryBase, IUserLoginRepository
    {
        public UserLoginRepository(DatabaseConnection connection)
            : base(connection)
        {
        }

        public virtual Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            var args = new { @UserId = user.Id, @LoginProvider = login.LoginProvider, @ProviderKey = login.ProviderKey };

            const string sql = @"
IF NOT EXISTS ( SELECT  TOP 1 1
                FROM    dbo.AspNetUserLogins u
                WHERE   ( u.UserId = @UserId )
                    AND ( u.LoginProvider = @LoginProvider )
                    AND ( u.ProviderKey = @ProviderKey ) )
BEGIN
    INSERT  INTO dbo.AspNetUserLogins ( UserId, LoginProvider, ProviderKey )
    VALUES  ( @UserId, @LoginProvider, @ProviderKey )
END
";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }

        public virtual Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            var args = new { @LoginProvider = login.LoginProvider, @ProviderKey = login.ProviderKey };

            const string sql = "SELECT u.* " + "FROM dbo.AspNetUsers u " + " JOIN dbo.AspNetUserLogins ul ON ul.UserId = u.Id " + "WHERE (ul.LoginProvider = @LoginProvider)"
                               + "  AND (ul.ProviderKey = @ProviderKey)";

            return this.Execute(db => db.QueryFirstOrDefaultAsync<IdentityUser>(sql, args));
        }

        public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            const string sql = "SELECT u.LoginProvider, u.ProviderKey " + "FROM dbo.AspNetUserLogins u " + "WHERE u.UserId = @0";

            var q = await this.Execute(db => db.QueryAsync<UserLoginRecord>(sql, user.Id));

            return q.SelectToList(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey));
        }

        public virtual Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            var args = new { @UserId = user.Id, @LoginProvider = login.LoginProvider, @ProviderKey = login.ProviderKey };

            const string sql = "DELETE u " + "FROM dbo.AspNetUserLogins u " + "WHERE (u.UserId = @UserId)" + " AND (u.LoginProvider = @LoginProvider)"
                               + " AND (u.ProviderKey = @ProviderKey)";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private class UserLoginRecord
        {
            public string LoginProvider { get; set; }

            public string ProviderKey { get; set; }
        }
    }
}
