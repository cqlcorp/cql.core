using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Cql.NativeMethods.Logon
{
    public static class ExtensionsForWindowsIndentity
    {
        public static string[] GetGroupNames(this WindowsIdentity identity)
        {
            return identity.Groups
                    .Select(x => x.Translate(typeof(NTAccount)).ToString())
                    .ToArray();
        }

        public static string[] GetGroupNames(this ClaimsPrincipal identity)
        {
            return
                identity.Claims
                    .Where(x => x.Type == ClaimTypes.GroupSid)
                    .Select(x => new SecurityIdentifier(x.Value).Translate(typeof(NTAccount)).ToString())
                    .ToArray();
        }
    }
}
