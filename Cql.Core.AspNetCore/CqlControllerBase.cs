using System;
using System.IO;
using System.Threading.Tasks;

using Cql.Core.Web;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Cql.Core.AspNetCore
{
    public abstract class CqlControllerBase : Controller
    {
        protected virtual async Task<IOperationResult> ExecuteServiceTask<TService, TResult>(Func<TService, Task<TResult>> serviceTask, TService service)
            where TResult : IOperationResult
        {
            return await serviceTask(service);
        }

        protected abstract void LogException(Exception ex, IOperationResult result = null);

        protected virtual async Task<IActionResult> OperationResponse(IOperationResult result)
        {
            switch (result.Result)
            {
                case OperationResultType.Ok:

                    if (result is ViewOperationResult viewResult)
                    {
                        return Ok(new
                                  {
                                      html = await RenderViewToString(viewResult.ViewName, viewResult.ViewModel, viewResult.IsPartial),
                                      data = viewResult.Data
                                  });
                    }

                    if (result.Data is FileContent fileContent)
                    {
                        if (fileContent.NotFound)
                        {
                            return NotFound();
                        }

                        return File(fileContent.ContentBytes, fileContent.ContentType, fileContent.FileName);
                    }

                    if (result.Data is RedirectToUrlOperationResult redirectToUrlResult)
                    {
                        return Redirect(redirectToUrlResult.Url);
                    }

                    if (result.Data is RedirectToActionOperationResult redirectToActionResult)
                    {
                        return RedirectToAction(redirectToActionResult.Action, redirectToActionResult.Controller, redirectToActionResult.RouteValues);
                    }

                    if (result.Data is RedirectToRouteOperationResult redirectToRouteResult)
                    {
                        return RedirectToRoute(redirectToRouteResult.Name, redirectToRouteResult.RouteValues);
                    }

                    return Ok(result.Data ?? new {message = "ok"});

                case OperationResultType.NotFound:
                    return NotFound();

                case OperationResultType.Unauthorized:
                    return Unauthorized();

                case OperationResultType.Error:
                    return BadRequest(result.Message);

                default:
                    throw new ArgumentOutOfRangeException(nameof(result.Result), result.Result, "Result type not supported.");
            }
        }

        protected virtual Task<string> RenderPartialViewAsync(object model = null)
        {
            return RenderPartialViewAsync(null, model);
        }

        protected virtual Task<string> RenderPartialViewAsync(string viewName, object model = null)
        {
            return RenderViewToString(viewName, model, true);
        }

        protected virtual Task<string> RenderViewAsync(object model = null)
        {
            return RenderViewAsync(null, model);
        }

        protected virtual Task<string> RenderViewAsync(string viewName, object model = null)
        {
            return RenderViewToString(viewName, model, false);
        }

        protected virtual TService ResolveService<TService>()
        {
            return HttpContext.RequestServices.GetRequiredService<TService>();
        }

        protected virtual Task<IActionResult> ServiceTask<TService>(Func<TService, Task<OperationResult>> serviceTask)
        {
            return ServiceTaskImpl(serviceTask);
        }

        protected virtual Task<IActionResult> ServiceTask<TService, TResult>(Func<TService, Task<OperationResult<TResult>>> serviceTask)
        {
            return ServiceTaskImpl(serviceTask);
        }

        private async Task<string> RenderViewToString(string viewName, object model, bool isPartial)
        {
            ActionContext actionContext = ControllerContext;

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = actionContext.GetActionName();
            }

            using (var sw = new StringWriter())
            {
                var engine = ResolveService<ICompositeViewEngine>();

                var viewResult = engine.FindView(actionContext, viewName, !isPartial);

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    new ViewDataDictionary(ViewData)
                    {
                        Model = model
                    },
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return sw.GetStringBuilder().ToString();
            }
        }

        private async Task<IActionResult> ServiceTaskImpl<TService, T>(Func<TService, Task<T>> serviceTask) where T : IOperationResult
        {
            IOperationResult result = null;

            try
            {
                var service = ResolveService<TService>();

                result = await ExecuteServiceTask(serviceTask, service);

                return await OperationResponse(result);
            }
            catch (Exception ex)
            {
                LogException(ex, result);

                var operationResult = OperationResult.Error(ex);

                Exception unauthorized = ex as UnauthorizedAccessException;

                if (unauthorized != null)
                {
                    operationResult = OperationResult.Unauthorized(ex.Message);
                }

                return await OperationResponse(operationResult);
            }
        }
    }
}
