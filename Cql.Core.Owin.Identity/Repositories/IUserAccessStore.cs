namespace Cql.Core.Owin.Identity.Repositories
{
    using System.Security.Principal;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    public interface IUserAccessStore
    {
        Task<IdentityResult> GrantAccess(IPrincipal currentUser, int userId);

        Task<IdentityResult> RevokeAccess(IPrincipal currentUser, int userId);
    }
}
