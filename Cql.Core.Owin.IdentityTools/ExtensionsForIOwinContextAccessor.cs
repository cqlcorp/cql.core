namespace Cql.Core.Owin.IdentityTools
{
    using System;
    using System.Security.Principal;

    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;

    public static class ExtensionsForIOwinContextAccessor
    {
        public static IAuthenticationManager GetAuthenticationManager(this IOwinContextAccessor owinContextAccessor)
        {
            return owinContextAccessor.OwinContext.Authentication;
        }

        public static IPrincipal GetCurrentUser(this IOwinContextAccessor owinContextAccessor)
        {
            return owinContextAccessor.OwinContext.Request.User;
        }

        public static IPrincipal GetCurrentUser(this IOwinContext owinContext)
        {
            return owinContext.Request.User;
        }

        public static T GetCurrentUserId<T>(this IOwinContextAccessor owinContextAccessor)
            where T : IConvertible
        {
            return owinContextAccessor.OwinContext.Request.User.Identity.GetUserId<T>();
        }

        public static int GetCurrentUserId(this IOwinContextAccessor owinContextAccessor)
        {
            return owinContextAccessor.GetCurrentUserId<int>();
        }

        public static T GetCurrentUserId<T>(this IOwinContext owinContext)
            where T : IConvertible
        {
            return owinContext.Request.User.Identity.GetUserId<T>();
        }

        public static int GetCurrentUserId(this IOwinContext owinContext)
        {
            return owinContext.GetCurrentUserId<int>();
        }
    }
}
