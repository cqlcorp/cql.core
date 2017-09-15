using Cql.Core.Web;

using JetBrains.Annotations;

namespace Cql.Core.AspNetCore
{
    public class ViewOperationResult : OperationResult
    {
        public static ViewOperationResult Partial([AspMvcPartialView] string viewName, object viewModel = null, object jsonData = null)
        {
            return new ViewOperationResult
                   {
                       Result = OperationResultType.Ok,
                       IsPartial = true,
                       ViewName = viewName,
                       ViewModel = viewModel,
                       Data = jsonData
                   };
        }

        public string ViewName { get; set; }

        public bool IsPartial { get; set; }

        public object ViewModel { get; set; }
    }
}
