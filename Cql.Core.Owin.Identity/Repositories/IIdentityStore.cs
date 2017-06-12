using Cql.Core.Owin.Identity.Types;

using Microsoft.AspNet.Identity;

namespace Cql.Core.Owin.Identity.Repositories
{
    public interface IIdentityStore :
        IUserLoginStore<IdentityUser, int>,
        IUserClaimStore<IdentityUser, int>,
        IUserRoleStore<IdentityUser, int>,
        IUserPasswordStore<IdentityUser, int>,
        IUserSecurityStampStore<IdentityUser, int>,
        IQueryableUserStore<IdentityUser, int>,
        IUserEmailStore<IdentityUser, int>,
        IUserPhoneNumberStore<IdentityUser, int>,
        IUserTwoFactorStore<IdentityUser, int>,
        IUserLockoutStore<IdentityUser, int>,
        IUserAccessStore,
        IClientRepository,
        IRefreshTokenRepository
    {
    }
}
