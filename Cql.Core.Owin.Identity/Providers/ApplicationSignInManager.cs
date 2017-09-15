namespace Cql.Core.Owin.Identity.Providers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Cql.Core.Owin.Identity.Types;

    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    public class ApplicationSignInManager : SignInManager<IdentityUser, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(IdentityUser user)
        {
            return ProviderUtils.GenerateUserIdentityAsync(this.UserManager, user);
        }
    }
}
