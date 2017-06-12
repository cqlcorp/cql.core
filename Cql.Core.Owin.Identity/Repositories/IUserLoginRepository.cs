using System.Collections.Generic;
using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Types;

using Microsoft.AspNet.Identity;

namespace Cql.Core.Owin.Identity.Repositories
{
    public interface IUserLoginRepository
    {
        Task AddLoginAsync(IdentityUser user, UserLoginInfo login);

        Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login);

        Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user);

        Task<IdentityUser> FindAsync(UserLoginInfo login);
    }
}
