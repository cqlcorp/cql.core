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
    public class RefreshTokenRepository : RepositoryBase, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DatabaseConnection connection)
            : base(connection)
        {
        }

        public virtual async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var args = new DynamicParameters(token);

            args.Add("@ret", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await this.Execute(db => db.ExecuteAsync("dbo.spRefreshToken_add", args, commandType: CommandType.StoredProcedure));

            return args.Get<int>("@ret") == 1;
        }

        public virtual Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var args = new { @Id = refreshTokenId };

            const string sql = "SELECT * FROM dbo.RefreshTokens WHERE Id = @Id";

            return this.Execute(db => db.QuerySingleOrDefaultAsync<RefreshToken>(sql, args));
        }

        public virtual async Task<IList<RefreshToken>> GetAllRefreshTokens()
        {
            var q = await this.Execute(db => db.QueryAsync<RefreshToken>("SELECT * FROM dbo.RefreshTokens"));

            return q.ToList();
        }

        public virtual async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var args = new { @Id = refreshTokenId };

            const string sql = "DELETE FROM dbo.RefreshTokens WHERE Id = @Id";

            return await this.Execute(db => db.ExecuteAsync(sql, args)) > 0;
        }

        public virtual Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            return this.RemoveRefreshToken(refreshToken.Id);
        }
    }
}
