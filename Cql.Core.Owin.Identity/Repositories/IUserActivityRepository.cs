using System.Threading.Tasks;

namespace Cql.Core.Owin.Identity.Repositories
{
    public interface IUserActivityRepository
    {
        Task UpdateLastActivity(int userId);
    }
}
