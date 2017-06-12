using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace Cql.Core.Owin.Identity.Repositories
{
    public interface IUserAccessStore
    {
        Task<IdentityResult> GrantAccess(IPrincipal currentUser, int userId);

        Task<IdentityResult> RevokeAccess(IPrincipal currentUser, int userId);
    }
}
