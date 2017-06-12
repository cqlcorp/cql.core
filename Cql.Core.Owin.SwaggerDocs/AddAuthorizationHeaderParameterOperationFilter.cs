using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Filters;

using Swashbuckle.Swagger;

namespace Cql.Core.Owin.SwaggerDocs
{
    public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline();
            var isAuthorized = filterPipeline
                .Select(filterInfo => filterInfo.Instance)
                .Any(filter => filter is IAuthorizationFilter);

            var allowAnonymous = apiDescription.GetControllerAndActionAttributes<AllowAnonymousAttribute>().Any();

            if (isAuthorized && !allowAnonymous)
            {
                LazyInitializer.EnsureInitialized(ref operation.parameters, () => new List<Parameter>());

                operation.parameters.Add(
                    new Parameter
                    {
                        name = "Authorization",
                        @in = "header",
                        description = "access token",
                        required = true,
                        type = "string",
                        example = "bearer {access_token}"
                    });
            }
        }
    }
}
