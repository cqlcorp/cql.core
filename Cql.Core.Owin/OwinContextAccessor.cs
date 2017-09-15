namespace Cql.Core.Owin
{
    using Microsoft.Owin;

    public class OwinContextAccessor : IOwinContextAccessor
    {
        public IOwinContext OwinContext { get; set; }
    }
}
