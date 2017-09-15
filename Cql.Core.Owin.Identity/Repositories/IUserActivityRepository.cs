namespace Cql.Core.Owin.Identity.Repositories
{
    using System.Threading.Tasks;

    public interface IUserActivityRepository
    {
        Task UpdateLastActivity(int userId);
    }
}
