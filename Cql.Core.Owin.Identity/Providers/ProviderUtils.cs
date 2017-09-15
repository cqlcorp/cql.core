namespace Cql.Core.Owin.Identity.Providers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Cql.Core.Owin.Identity.Types;

    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security.OAuth;

    public static class ProviderUtils
    {
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<IdentityUser, int> manager, IdentityUser user)
        {
            var identity = await manager.CreateIdentityAsync(user, OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            return identity;
        }
    }
}
