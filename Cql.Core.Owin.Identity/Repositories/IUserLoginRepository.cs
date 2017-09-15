namespace Cql.Core.Owin.Identity.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Cql.Core.Owin.Identity.Types;

    using Microsoft.AspNet.Identity;

    public interface IUserLoginRepository
    {
        Task AddLoginAsync(IdentityUser user, UserLoginInfo login);

        Task<IdentityUser> FindAsync(UserLoginInfo login);

        Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user);

        Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login);
    }
}
