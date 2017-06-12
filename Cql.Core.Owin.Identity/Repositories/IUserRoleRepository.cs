using System.Collections.Generic;
using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Types;

namespace Cql.Core.Owin.Identity.Repositories
{
    public interface IUserRoleRepository
    {
        Task AddToRoleAsync(IdentityUser user, string roleName);

        Task<IList<string>> GetRolesAsync(IdentityUser user);

        Task<bool> IsInRoleAsync(IdentityUser user, string roleName);

        Task RemoveFromRoleAsync(IdentityUser user, string roleName);
    }
}
