using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Repositories;
using Cql.Core.Owin.Identity.Types;

using Microsoft.AspNet.Identity;

namespace Cql.Core.Owin.Identity
{
    public class RoleStore : IRoleStore<IdentityRole, int>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleStore(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public virtual Task CreateAsync(IdentityRole role)
        {
            return _roleRepository.CreateAsync(role);
        }

        public virtual Task DeleteAsync(IdentityRole role)
        {
            return _roleRepository.DeleteAsync(role);
        }

        public virtual void Dispose()
        {
        }

        public virtual Task<IdentityRole> FindByIdAsync(int roleId)
        {
            return _roleRepository.FindByIdAsync(roleId);
        }

        public virtual Task<IdentityRole> FindByNameAsync(string roleName)
        {
            return _roleRepository.FindByNameAsync(roleName);
        }

        public virtual Task UpdateAsync(IdentityRole role)
        {
            return _roleRepository.UpdateAsync(role);
        }
    }
}
