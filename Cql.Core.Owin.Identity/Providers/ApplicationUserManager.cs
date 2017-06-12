using Cql.Core.Owin.Identity.Types;

using Microsoft.AspNet.Identity;

namespace Cql.Core.Owin.Identity.Providers
{
    public class ApplicationUserManager : UserManager<IdentityUser, int>
    {
        public ApplicationUserManager(IUserStore<IdentityUser, int> store)
            : base(store) {}
    }
}
