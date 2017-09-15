namespace Cql.Core.Owin.Identity.Repositories
{
    using System.Threading.Tasks;

    using Cql.Core.Owin.Identity.Types;

    public interface IClientRepository
    {
        Task<Client> FindClientByIdAsync(string clientId);
    }
}
