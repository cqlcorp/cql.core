using Microsoft.Owin;

namespace Cql.Core.Owin
{
    public class OwinContextAccessor : IOwinContextAccessor
    {
        public IOwinContext OwinContext { get; set; }
    }
}
