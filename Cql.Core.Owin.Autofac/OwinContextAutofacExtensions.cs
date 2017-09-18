// ***********************************************************************
// Assembly         : Cql.Core.Owin.Autofac
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="OwinContextAutofacExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Owin.Autofac
{
    using global::Autofac;
    using global::Autofac.Integration.Owin;

    using JetBrains.Annotations;

    using Microsoft.Owin;

    /// <summary>
    /// Class OwinContextAutofacExtensions.
    /// </summary>
    public static class OwinContextAutofacExtensions
    {
        /// <summary>
        /// Resolves an instance of the specified <typeparamref name="TService"/> from the current Autofac Lifetime scope container.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="owinContext">The owin context.</param>
        /// <returns>An instance of TService</returns>
        public static TService Resolve<TService>([NotNull] this IOwinContext owinContext)
        {
            return owinContext.GetAutofacLifetimeScope().Resolve<TService>();
        }

        /// <summary>
        /// Resolves an instance of the specified <typeparamref name="TService" /> from the current Autofac Lifetime scope container.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="owinContext">The owin context.</param>
        /// <param name="serviceKey">The service key.</param>
        /// <returns>An instance of TService</returns>
        [NotNull]
        public static TService ResolveKeyed<TService>([NotNull] this IOwinContext owinContext, [NotNull] object serviceKey)
        {
            return owinContext.GetAutofacLifetimeScope().ResolveKeyed<TService>(serviceKey);
        }

        /// <summary>
        /// Resolves an instance of the specified <typeparamref name="TService" /> from the current Autofac Lifetime scope container.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="owinContext">The owin context.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>An instance of TService</returns>
        public static TService ResolveNamed<TService>([NotNull] this IOwinContext owinContext, [NotNull] string serviceName)
        {
            return owinContext.GetAutofacLifetimeScope().ResolveNamed<TService>(serviceName);
        }
    }
}
