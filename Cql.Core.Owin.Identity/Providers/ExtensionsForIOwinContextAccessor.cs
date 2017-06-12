using Cql.Core.Owin.Identity.Types;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Cql.Core.Owin.Identity.Providers
{
    public static class ExtensionsForIOwinContextAccessor
    {
        public static UserManager<IdentityUser, int> GetUserManager(this IOwinContextAccessor owinContextAccessor)
        {
            return owinContextAccessor.OwinContext.Get<ApplicationUserManager>();
        }
    }
}
