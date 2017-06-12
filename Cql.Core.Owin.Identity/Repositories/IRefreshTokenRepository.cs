using System.Collections.Generic;
using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Types;

namespace Cql.Core.Owin.Identity.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<bool> AddRefreshToken(RefreshToken token);

        Task<RefreshToken> FindRefreshToken(string refreshTokenId);

        Task<IList<RefreshToken>> GetAllRefreshTokens();

        Task<bool> RemoveRefreshToken(string refreshTokenId);

        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);
    }
}
