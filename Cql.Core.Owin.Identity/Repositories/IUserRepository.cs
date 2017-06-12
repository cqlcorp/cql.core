using System;
using System.Linq;
using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Types;

namespace Cql.Core.Owin.Identity.Repositories
{
    public interface IUserRepository
    {
        IQueryable<IdentityUser> Users { get; }

        Task CreateAsync(IdentityUser user);

        Task DeleteAsync(IdentityUser user);

        Task<IdentityUser> FindByEmailAsync(string email);

        Task<IdentityUser> FindByGuidAsync(Guid guid);

        Task<IdentityUser> FindByIdAsync(int userId);

        Task<IdentityUser> FindByNameAsync(string userName);

        Task UpdateAsync(IdentityUser user);
    }
}
