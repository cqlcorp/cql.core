namespace Cql.Core.Owin.IdentityData
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Cql.Core.Owin.Identity.Repositories;
    using Cql.Core.Owin.Identity.Types;
    using Cql.Core.SqlServer;

    using Dapper;

    [SuppressMessage("ReSharper", "RedundantVerbatimPrefix")]
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public class ClaimRepository : RepositoryBase, IClaimRepository
    {
        public ClaimRepository(DatabaseConnection connection)
            : base(connection)
        {
        }

        public virtual Task AddClaimAsync(int userId, Claim claim)
        {
            var args = new { @UserId = userId, @ClaimType = claim.Type, @ClaimValue = claim.Value };

            const string sql = "INSERT INTO dbo.AspNetUserClaims (UserId, ClaimType, ClaimValue)" + "VALUES (@UserId, @ClaimType, @ClaimValue)";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }

        public virtual Task AddIfNotExistsClaimAsync(int userId, Claim claim)
        {
            var args = new { @UserId = userId, @ClaimType = claim.Type, @ClaimValue = claim.Value };

            const string sql = @"
IF NOT EXISTS ( SELECT TOP 1
                    1
            FROM    dbo.AspNetUserClaims
            WHERE   UserId = @UserId
                    AND ClaimType = @ClaimType
                    AND ClaimValue = @ClaimValue )
    BEGIN
        INSERT  INTO dbo.AspNetUserClaims
                ( UserId, ClaimType, ClaimValue )
        VALUES  ( @UserId, @ClaimType, @ClaimValue )
    END;
";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }

        public virtual Task AddOrUpdateClaimAsync(int userId, Claim claim)
        {
            var args = new { @UserId = userId, @ClaimType = claim.Type, @ClaimValue = claim.Value };

            const string sql = @"
IF EXISTS ( SELECT TOP 1
                    1
            FROM    dbo.AspNetUserClaims
            WHERE   UserId = @UserId
                    AND ClaimType = @ClaimType )
    BEGIN
        UPDATE  dbo.AspNetUserClaims
        SET     ClaimValue = @ClaimValue
        WHERE   UserId = @UserId
                AND ClaimType = @ClaimType;
    END
ELSE
    BEGIN
        INSERT  INTO dbo.AspNetUserClaims
                ( UserId, ClaimType, ClaimValue )
        VALUES  ( @UserId, @ClaimType, @ClaimValue )
    END;
";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }

        public virtual async Task<IList<Claim>> GetClaimsAsync(int userId)
        {
            var args = new { @UserId = userId };

            const string sql = "SELECT * FROM dbo.AspNetUserClaims " + "WHERE UserId = @UserId";

            var q = await this.Execute(db => db.QueryAsync<IdentityClaim>(sql, args));

            return q.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }

        public virtual async Task<string[]> GetClaimValuesAsync(int userId, string claimType)
        {
            var args = new { @UserId = userId, @ClaimType = claimType };

            const string sql = "SELECT" + " ClaimValue " + "FROM dbo.AspNetUserClaims " + "WHERE" + " UserId = @UserId" + " AND" + " ClaimType = @ClaimType";

            var q = await this.Execute(db => db.QueryAsync<string>(sql, args));

            return q.ToArray();
        }

        public virtual Task RemoveAllClaimsOfTypeAsync(int userId, string claimType)
        {
            var args = new { @UserId = userId, @ClaimType = claimType };

            const string sql = "DELETE FROM dbo.AspNetUserClaims " + "WHERE UserId = @UserId AND ClaimType = @ClaimType";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }

        public virtual Task RemoveClaimAsync(int userId, Claim claim)
        {
            var args = new { @UserId = userId, @ClaimType = claim.Type, @ClaimValue = claim.Value };

            const string sql = "DELETE FROM dbo.AspNetUserClaims " + "WHERE UserId = @UserId AND ClaimType = @ClaimType AND ClaimValue = @ClaimValue";

            return this.Execute(db => db.ExecuteAsync(sql, args));
        }
    }
}
