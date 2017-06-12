using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Types;

namespace Cql.Core.Owin.Identity.Repositories
{
    public interface IClientRepository
    {
        Task<Client> FindClientByIdAsync(string clientId);
    }
}
