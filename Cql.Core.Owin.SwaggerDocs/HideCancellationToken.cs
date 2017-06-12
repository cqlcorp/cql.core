using System.Linq;
using System.Threading;
using System.Web.Http.Description;

using Microsoft.Win32.SafeHandles;

using Swashbuckle.Swagger;

namespace Cql.Core.Owin.SwaggerDocs
{
    public class HideCancellationToken : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            apiDescription.ParameterDescriptions
                .Where(pd =>
                    pd.ParameterDescriptor.ParameterType == typeof(CancellationToken) ||
                    pd.ParameterDescriptor.ParameterType == typeof(WaitHandle) ||
                    pd.ParameterDescriptor.ParameterType == typeof(SafeWaitHandle))
                .ToList()
                .ForEach(
                    pd =>
                    {
                        if (operation.parameters == null)
                        {
                            return;
                        }

                        var cancellationTokenParameter = operation.parameters.Single(p => p.name == pd.Name);
                        operation.parameters.Remove(cancellationTokenParameter);
                    });
        }
    }
}
