namespace Cql.Core.Owin.Identity.Providers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Cql.Core.Owin.Identity.Types;
    using Cql.Core.Owin.IdentityTools;

    using Microsoft.AspNet.Identity;

    public static class ExtensionsForUserManager
    {
        public static Task AddClaimAsync(this UserManager<IdentityUser, int> userManager, int userId, Enum claimType, string value)
        {
            return userManager.AddClaimAsync(userId, ClaimFactory.Create(claimType, value));
        }

        public static Task AddClaimAsync(this UserManager<IdentityUser, int> userManager, int userId, Enum claimType, Enum value)
        {
            return userManager.AddClaimAsync(userId, ClaimFactory.Create(claimType, value));
        }

        public static Task AddToRolesAsync(this UserManager<IdentityUser, int> userManager, IUserId user, Enum roles)
        {
            return AddToRolesAsync(userManager, user.UserId, roles);
        }

        public static async Task AddToRolesAsync(this UserManager<IdentityUser, int> userManager, int userId, Enum roles)
        {
            var currentRoles = await userManager.GetRolesAsync(userId);

            var roleList = roles.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Except(currentRoles).ToList();

            roleList.RemoveAll(roleName => string.Equals(roleName, "None", StringComparison.OrdinalIgnoreCase));

            if (roleList.Count > 0)
            {
                await userManager.AddToRolesAsync(userId, roleList.ToArray());
            }
        }
    }
}
