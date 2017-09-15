namespace Cql.Core.Owin.Autofac
{
    using System.Reflection;

    using global::Autofac;
    using global::Autofac.Extras.CommonServiceLocator;

    using global::Owin;

    using Microsoft.Owin;
    using Microsoft.Practices.ServiceLocation;

    public static class AutofacBuilderExtensions
    {
        public static IContainer BuildContainer(this ContainerBuilder builder, IAppBuilder app)
        {
            var container = builder.Build();

            app.UseAutofacMiddleware(container);

            // Support components that depend on Cql.Core.ServiceLocation
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));

            return container;
        }

        public static void RegisterControllers(this ContainerBuilder builder, Assembly webAssembly)
        {
            builder.RegisterAssemblyTypes(webAssembly).Where(x => x.Name.EndsWith("Controller")).AsSelf();
        }

        public static void RegisterOwinContextAccessor(this ContainerBuilder builder)
        {
            builder.RegisterType<OwinContextAccessor>().OnActivating(
                e =>
                    {
                        IOwinContext context;
                        if (e.Context.TryResolve(out context))
                        {
                            e.Instance.OwinContext = context;
                        }
                    }).As<IOwinContextAccessor>();
        }
    }
}
