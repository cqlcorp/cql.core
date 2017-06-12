using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Repositories;
using Cql.Core.Owin.Identity.Types;
using Cql.Core.SqlServer;

using Dapper;

namespace Cql.Core.Owin.IdentityData
{
    [SuppressMessage("ReSharper", "RedundantVerbatimPrefix")]
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(DatabaseConnection connection) : base(connection)
        {
        }

        public virtual IQueryable<IdentityUser> Users
        {
            get { throw new NotImplementedException(); }
        }

        public virtual async Task CreateAsync(IdentityUser user)
        {
            var args = GetArgsFromUser(user);

            const string sql = "INSERT INTO dbo.AspNetUsers (" +
                               " Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber," +
                               " PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDate, LockoutEnabled, AccessFailedCount, UserName) " +
                               "OUTPUT INSERTED.* " +
                               "VALUES (" +
                               " @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumber," +
                               " @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDate, @LockoutEnabled, @AccessFailedCount, @UserName)";

            var createdUser = await Execute(db => db.QuerySingleAsync<IdentityUser>(sql, args));

            user.Id = createdUser.Id;
            user.Guid = createdUser.Guid;
            user.UserName = createdUser.UserName;
            user.Email = createdUser.Email;
            user.EmailConfirmed = createdUser.EmailConfirmed;
            user.PasswordHash = createdUser.PasswordHash;
            user.SecurityStamp = createdUser.SecurityStamp;
            user.PhoneNumber = createdUser.PhoneNumber;
            user.PhoneNumberConfirmed = createdUser.PhoneNumberConfirmed;
            user.TwoFactorEnabled = createdUser.TwoFactorEnabled;
            user.LockoutEndDate = createdUser.LockoutEndDate;
            user.LockoutEnabled = createdUser.LockoutEnabled;
            user.AccessFailedCount = createdUser.AccessFailedCount;
        }

        public virtual Task DeleteAsync(IdentityUser user)
        {
            var args = new
            {
                @Id = user.Id
            };

            const string sql = "DELETE FROM dbo.AspNetUsers WHERE Id = @Id";

            return Execute(db => db.ExecuteAsync(sql, args));
        }

        public virtual Task<IdentityUser> FindByEmailAsync(string email)
        {
            var args = new
            {
                @Email = email
            };

            const string sql = "SELECT TOP 1 * " +
                               "FROM dbo.AspNetUsers u " +
                               "WHERE u.Email LIKE @Email " +
                               "ORDER BY u.UserName";

            return Execute(db => db.QueryFirstOrDefaultAsync<IdentityUser>(sql, args));
        }

        public virtual Task<IdentityUser> FindByGuidAsync(Guid guid)
        {
            var args = new
            {
                @Guid = guid
            };

            const string sql = "SELECT * " +
                               "FROM dbo.AspNetUsers " +
                               "WHERE Guid = @Guid";

            return Execute(db => db.QuerySingleOrDefaultAsync<IdentityUser>(sql, args));
        }

        public virtual Task<IdentityUser> FindByIdAsync(int userId)
        {
            var args = new
            {
                @Id = userId
            };

            const string sql = "SELECT * " +
                               "FROM dbo.AspNetUsers " +
                               "WHERE Id = @Id";

            return Execute(db => db.QuerySingleOrDefaultAsync<IdentityUser>(sql, args));
        }

        public virtual Task<IdentityUser> FindByNameAsync(string userName)
        {
            var args = new
            {
                @UserName = userName
            };

            const string sql = "SELECT * " +
                               "FROM dbo.AspNetUsers " +
                               "WHERE UserName = @UserName";

            return Execute(db => db.QuerySingleOrDefaultAsync<IdentityUser>(sql, args));
        }

        public virtual Task UpdateAsync(IdentityUser user)
        {
            var args = GetArgsFromUser(user);

            const string sql = "UPDATE dbo.AspNetUsers " +
                               "SET " +
                               " Email = @Email," +
                               " EmailConfirmed = @EmailConfirmed," +
                               " PasswordHash = @PasswordHash," +
                               " SecurityStamp = @SecurityStamp," +
                               " PhoneNumber = @PhoneNumber," +
                               " PhoneNumberConfirmed = @PhoneNumberConfirmed," +
                               " TwoFactorEnabled = @TwoFactorEnabled," +
                               " LockoutEndDate = @LockoutEndDate," +
                               " LockoutEnabled = @LockoutEnabled," +
                               " AccessFailedCount = @AccessFailedCount," +
                               " UserName = @UserName " +
                               "WHERE Id = @Id";

            return Execute(db => db.ExecuteAsync(sql, args));
        }

        private static object GetArgsFromUser(IdentityUser user)
        {
            return new
            {
                @Id = user.Id,
                @Email = user.Email,
                @EmailConfirmed = user.EmailConfirmed,
                @PasswordHash = user.PasswordHash,
                @SecurityStamp = user.SecurityStamp,
                @PhoneNumber = user.PhoneNumber ?? "",
                @PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                @TwoFactorEnabled = user.TwoFactorEnabled,
                @LockoutEndDate = user.LockoutEndDate,
                @LockoutEnabled = user.LockoutEnabled,
                @AccessFailedCount = user.AccessFailedCount,
                @UserName = user.UserName
            };
        }
    }
}
