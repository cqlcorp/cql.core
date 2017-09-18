// ***********************************************************************
// Assembly         : Cql.Core.Owin.Autofac
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="AutofacBuilderExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Owin.Autofac
{
    using System.Reflection;

    using global::Autofac;
    using global::Autofac.Extras.CommonServiceLocator;

    using global::Owin;

    using JetBrains.Annotations;

    using Microsoft.Owin;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Class AutofacBuilderExtensions.
    /// </summary>
    public static class AutofacBuilderExtensions
    {
        /// <summary>
        /// Builds the container.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="app">The application.</param>
        /// <returns>
        ///     <see cref="IContainer" />
        /// </returns>
        [NotNull]
        public static IContainer BuildContainer([NotNull] this ContainerBuilder builder, [NotNull] IAppBuilder app)
        {
            var container = builder.Build();

            app.UseAutofacMiddleware(container);

            // Support components that depend on Cql.Core.ServiceLocation
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));

            return container;
        }

        /// <summary>
        /// Registers the controllers.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="webAssembly">The web assembly.</param>
        public static void RegisterControllers([NotNull] this ContainerBuilder builder, [NotNull] Assembly webAssembly)
        {
            builder.RegisterAssemblyTypes(webAssembly).Where(x => x.Name.EndsWith("Controller")).AsSelf();
        }

        /// <summary>
        /// Registers the owin context accessor.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void RegisterOwinContextAccessor([NotNull] this ContainerBuilder builder)
        {
            builder.RegisterType<OwinContextAccessor>().OnActivating(
                e =>
                    {
                        if (e.Context.TryResolve(out IOwinContext context))
                        {
                            e.Instance.OwinContext = context;
                        }
                    }).As<IOwinContextAccessor>();
        }
    }
}
