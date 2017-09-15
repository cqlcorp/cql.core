namespace Cql.Core.ServiceLocation
{
    using System;

    using Microsoft.Practices.ServiceLocation;

    public static class ServiceResolver
    {
        public static IServiceLocator ServiceLocator => Microsoft.Practices.ServiceLocation.ServiceLocator.Current;

        /// <summary>
        /// Get an instance of the given named <typeparamref name="TService" />.
        /// </summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <exception cref="T:Microsoft.Practices.ServiceLocation.ActivationException">
        /// if there is are errors resolving
        /// the service instance.
        /// </exception>
        /// <returns>The requested service instance.</returns>
        public static TService Resolve<TService>()
        {
            return ServiceLocator.GetInstance<TService>();
        }

        /// <summary>
        /// Get an instance of the given named <typeparamref name="TService" />.
        /// </summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <param name="key">Name the object was registered with.</param>
        /// <exception cref="T:Microsoft.Practices.ServiceLocation.ActivationException">
        /// if there is are errors resolving
        /// the service instance.
        /// </exception>
        /// <returns>The requested service instance.</returns>
        public static TService Resolve<TService>(string key)
        {
            return ServiceLocator.GetInstance<TService>(key);
        }

        /// <summary>
        /// Get an instance of the given named <paramref name="serviceType" />.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <exception cref="T:Microsoft.Practices.ServiceLocation.ActivationException">
        /// if there is an error resolving
        /// the service instance.
        /// </exception>
        /// <returns>The requested service instance.</returns>
        public static object Resolve(Type serviceType)
        {
            return ServiceLocator.GetInstance(serviceType);
        }

        /// <summary>
        /// Get an instance of the given named <paramref name="serviceType" />.
        /// </summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <param name="key">Name the object was registered with.</param>
        /// <exception cref="T:Microsoft.Practices.ServiceLocation.ActivationException">
        /// if there is an error resolving
        /// the service instance.
        /// </exception>
        /// <returns>The requested service instance.</returns>
        public static object Resolve(Type serviceType, string key)
        {
            return ServiceLocator.GetInstance(serviceType, key);
        }
    }
}
