using Autofac;
using Autofac.Integration.Owin;

using Microsoft.Owin;

namespace Cql.Core.Owin.Autofac
{
    public static class OwinContextAutofacExtensions
    {
        public static T Resolve<T>(this IOwinContext owinContext)
        {
            return owinContext.GetAutofacLifetimeScope().Resolve<T>();
        }

        public static T ResolveKeyed<T>(this IOwinContext owinContext, object serviceKey)
        {
            return owinContext.GetAutofacLifetimeScope().ResolveKeyed<T>(serviceKey);
        }

        public static T ResolveNamed<T>(this IOwinContext owinContext, string serviceName)
        {
            return owinContext.GetAutofacLifetimeScope().ResolveNamed<T>(serviceName);
        }
    }
}