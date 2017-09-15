namespace Cql.Core.Owin.Identity.Providers
{
    using Cql.Core.Owin.Identity.Types;

    using Microsoft.AspNet.Identity;

    public class ApplicationUserManager : UserManager<IdentityUser, int>
    {
        public ApplicationUserManager(IUserStore<IdentityUser, int> store)
            : base(store)
        {
        }
    }
}
