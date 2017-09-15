namespace Cql.Core.Owin.Identity.Repositories
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IClaimRepository
    {
        Task AddClaimAsync(int userId, Claim claim);

        Task AddIfNotExistsClaimAsync(int userId, Claim claim);

        Task AddOrUpdateClaimAsync(int userId, Claim claim);

        Task<IList<Claim>> GetClaimsAsync(int userId);

        Task<string[]> GetClaimValuesAsync(int userId, string claimType);

        Task RemoveAllClaimsOfTypeAsync(int userId, string claimType);

        Task RemoveClaimAsync(int userId, Claim claim);
    }
}
