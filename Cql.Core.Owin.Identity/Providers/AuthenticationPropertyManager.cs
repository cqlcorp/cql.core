namespace Cql.Core.Owin.Identity.Providers
{
    using System.Collections.Generic;
    using System.Security.Principal;

    using Cql.Core.Owin.Identity.Types;
    using Cql.Core.Owin.IdentityTools;

    using Microsoft.Owin.Security;

    internal static class AuthenticationPropertyManager
    {
        /// <summary>
        /// Adds properties that are stored within the encrypted token value.
        /// </summary>
        public static AuthenticationProperties GenerateAuthenticationProperties(IIdentity claimsIdentity, IdentityUser user, string clientId)
        {
            var roles = string.Join(",", claimsIdentity.GetRoleNames());

            if (string.IsNullOrEmpty(roles))
            {
                roles = "None";
            }

            var displayName = claimsIdentity.GetClaimValue(DefaultClaimTypes.DisplayName);

            IDictionary<string, string> data =
                new Dictionary<string, string> { ["userName"] = user.UserName, ["displayName"] = displayName ?? user.Email, ["roles"] = roles, [".client_id"] = clientId };

            return new AuthenticationProperties(data);
        }
    }
}
