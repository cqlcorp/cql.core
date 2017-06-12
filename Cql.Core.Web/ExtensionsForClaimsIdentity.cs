using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Cql.Core.Web
{
    public static class ExtensionsForClaimsIdentity
    {
        public static string[] GetRoleNames(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            return claimsIdentity?.Claims
                           .Where(x => x.Type == ClaimTypes.Role)
                           .Select(x => x.Value)
                           .ToArray() ?? new string[]{};
        }
    }
}
