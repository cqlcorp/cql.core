using System.Security.Claims;
using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Types;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Cql.Core.Owin.Identity.Providers
{
    public class ApplicationSignInManager : SignInManager<IdentityUser, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) {}

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(IdentityUser user)
        {
            return ProviderUtils.GenerateUserIdentityAsync(UserManager, user);
        }
    }
}
