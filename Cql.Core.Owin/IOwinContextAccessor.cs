namespace Cql.Core.Owin
{
    using Microsoft.Owin;

    public interface IOwinContextAccessor
    {
        IOwinContext OwinContext { get; set; }
    }
}
