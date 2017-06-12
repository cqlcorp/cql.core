using Cql.Core.Owin.Identity.Types;

using Microsoft.AspNet.Identity;

namespace Cql.Core.Owin.Identity.Providers
{
    public class ApplicationRoleManager : RoleManager<IdentityRole, int>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, int> roleStore)
            : base(roleStore) {}
    }
}
