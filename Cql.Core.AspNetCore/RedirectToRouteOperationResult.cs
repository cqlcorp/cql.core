using Cql.Core.Web;

namespace Cql.Core.AspNetCore
{
    public class RedirectToRouteOperationResult : OperationResult
    {
        public RedirectToRouteOperationResult(object routeValues) : this(null, routeValues)
        {
        }

        public RedirectToRouteOperationResult(string name) : this(name, null)
        {
        }

        public RedirectToRouteOperationResult(string name, object routeValues)
        {
            Name = name;
            RouteValues = routeValues;
        }

        public string Name { get; set; }

        public object RouteValues { get; set; }
    }
}
