using System.Threading.Tasks;

namespace Cql.Core.Owin.Identity.Repositories
{
    public interface IUserAccessRepository
    {
        Task GrantAccess(int userId);

        Task RevokeAccess(int userId, int revokedByUserId);
    }
}
