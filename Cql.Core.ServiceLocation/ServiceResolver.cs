// ***********************************************************************
// Assembly         : Cql.Core.ServiceLocation
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="ServiceResolver.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.ServiceLocation
{
    using System;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// A wrapping class around the <see cref="Microsoft.Practices.ServiceLocation.ServiceLocator"/> implementation.
    /// </summary>
    public static class ServiceResolver
    {
        /// <summary>
        /// Gets the service locator.
        /// </summary>
        /// <value>The service locator.</value>
        public static IServiceLocator ServiceLocator => Microsoft.Practices.ServiceLocation.ServiceLocator.Current;

        /// <summary>
        /// Get an instance of the given named <typeparamref name="TService" />.
        /// </summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <returns>The requested service instance.</returns>
        /// <exception cref="T:Microsoft.Practices.ServiceLocation.ActivationException">
        /// if there is are errors resolving
        /// the service instance.
        /// </exception>
        public static TService Resolve<TService>()
        {
            return ServiceLocator.GetInstance<TService>();
        }

        /// <summary>
        /// Get an instance of the given named <typeparamref name="TService" />.
        /// </summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <param name="key">Name the object was registered with.</param>
        /// <returns>The requested service instance.</returns>
        /// <exception cref="T:Microsoft.Practices.ServiceLocation.ActivationException">
        /// if there is are errors resolving
        /// the service instance.
        /// </exception>
        public static TService Resolve<TService>(string key)
        {
            return ServiceLocator.GetInstance<TService>(key);
        }

        /// <summary>
        /// Get an instance of the given named <paramref name="serviceType" />.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <returns>The requested service instance.</returns>
        /// <exception cref="T:Microsoft.Practices.ServiceLocation.ActivationException">
        /// if there is an error resolving
        /// the service instance.
        /// </exception>
        public static object Resolve(Type serviceType)
        {
            return ServiceLocator.GetInstance(serviceType);
        }

        /// <summary>
        /// Get an instance of the given named <paramref name="serviceType" />.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <param name="key">Name the object was registered with.</param>
        /// <returns>The requested service instance.</returns>
        /// <exception cref="T:Microsoft.Practices.ServiceLocation.ActivationException">
        /// if there is an error resolving
        /// the service instance.
        /// </exception>
        public static object Resolve(Type serviceType, string key)
        {
            return ServiceLocator.GetInstance(serviceType, key);
        }
    }
}
