using Cql.Core.Web;

namespace Cql.Core.AspNetCore
{
    public class RedirectToActionOperationResult : OperationResult
    {
        public RedirectToActionOperationResult(string action) : this(action, null)
        {
        }

        public RedirectToActionOperationResult(string action, string controller) : this(action, controller, null)
        {
        }

        public RedirectToActionOperationResult(string action, object routeValues) : this(action, null, routeValues)
        {
        }

        public RedirectToActionOperationResult(string action, string controller, object routeValues)
        {
            Action = action;
            Controller = controller;
            RouteValues = routeValues;
        }

        public string Action { get; set; }

        public string Controller { get; set; }

        public object RouteValues { get; set; }
    }
}
