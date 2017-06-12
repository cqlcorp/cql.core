using Microsoft.Owin;

namespace Cql.Core.Owin
{
    public interface IOwinContextAccessor
    {
        IOwinContext OwinContext { get; set; }
    }
}
