namespace Cql.Core.Owin
{
    using System.Net.Http.Extensions.Compression.Core.Compressors;
    using System.Web.Http;

    using global::Owin;

    using Microsoft.AspNet.WebApi.Extensions.Compression.Server.Owin;
    using Microsoft.Owin.Cors;
    using Microsoft.Owin.Security.OAuth;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class DefaultHttpConfigurationBuilder
    {
        public virtual HttpConfiguration CreateDefaultHttpConfiguration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();

            this.ConfigureCorsSupport(httpConfiguration, app);

            this.ConfigureWebApiAuthorization(httpConfiguration);

            this.ConfigureRoutes(httpConfiguration);

            this.ConfigureJsonFormatting(httpConfiguration);

            this.EnableSupportForGzip(httpConfiguration);

            this.SetErrorPolicy(httpConfiguration);

            return httpConfiguration;
        }

        protected virtual void ConfigureCorsSupport(HttpConfiguration httpConfiguration, IAppBuilder app)
        {
            httpConfiguration.EnableCors();
            app.UseCors(CorsOptions.AllowAll);
        }

        protected virtual void ConfigureJsonFormatting(HttpConfiguration httpConfiguration)
        {
            var jsonFormatter = httpConfiguration.Formatters.JsonFormatter;

            jsonFormatter.SerializerSettings = new JsonSerializerSettings
                                                   {
                                                       ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                       DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                                       DateTimeZoneHandling = DateTimeZoneHandling.Local,
                                                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                   };
        }

        protected virtual void ConfigureRoutes(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MapHttpAttributeRoutes();

            httpConfiguration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
        }

        /// <summary>
        /// Configure Web API to use only bearer token authentication.
        /// </summary>
        protected virtual void ConfigureWebApiAuthorization(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.SuppressDefaultHostAuthentication();
            httpConfiguration.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            httpConfiguration.Filters.Add(new AuthorizeAttribute());
        }

        /// <summary>
        /// Enable reading and writing GZip compressed data
        /// </summary>
        protected virtual void EnableSupportForGzip(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MessageHandlers.Insert(0, new OwinServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
        }

        protected virtual void SetErrorPolicy(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
        }
    }
}
