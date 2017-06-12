using System.Threading.Tasks;

using Microsoft.Owin;

namespace Cql.Core.Owin.WebPack
{
    public delegate Task<bool> HandleDefaultReponseDelegate(IOwinContext context, string defaultFilePath);
}
