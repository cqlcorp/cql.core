namespace Cql.Core.Owin.Identity.Repositories
{
    using System.Threading.Tasks;

    using Cql.Core.Owin.Identity.Types;

    public interface IRoleRepository
    {
        Task CreateAsync(IdentityRole role);

        Task DeleteAsync(IdentityRole role);

        Task<IdentityRole> FindByIdAsync(int roleId);

        Task<IdentityRole> FindByNameAsync(string roleName);

        Task UpdateAsync(IdentityRole role);
    }
}
