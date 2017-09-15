namespace Cql.Core.Owin.Identity.Repositories
{
    using System.Threading.Tasks;

    public interface IUserAccessRepository
    {
        Task GrantAccess(int userId);

        Task RevokeAccess(int userId, int revokedByUserId);
    }
}
