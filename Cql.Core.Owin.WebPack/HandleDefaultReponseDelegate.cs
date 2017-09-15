namespace Cql.Core.Owin.WebPack
{
    using System.Threading.Tasks;

    using Microsoft.Owin;

    public delegate Task<bool> HandleDefaultReponseDelegate(IOwinContext context, string defaultFilePath);
}
