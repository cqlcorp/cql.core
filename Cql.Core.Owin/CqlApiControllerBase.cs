namespace Cql.Core.Owin
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Cql.Core.ServiceLocation;
    using Cql.Core.Web;

    using JetBrains.Annotations;

    using Microsoft.Owin;

    using Newtonsoft.Json.Linq;

    public abstract class CqlApiControllerBase : ApiController
    {
        private IOwinContext _owinContext;

        /// <summary>
        /// The currently signed in user.
        /// </summary>
        [CanBeNull]
        public new ClaimsPrincipal User => base.User as ClaimsPrincipal;

        /// <summary>
        /// Gets the current <see cref="IOwinContext" /> for the current request.
        /// </summary>
        [NotNull]
        protected IOwinContext OwinContext => this.Request == null
                                                  ? new OwinContext()
                                                  : LazyInitializer.EnsureInitialized(ref this._owinContext, () => this.Request.GetOwinContext() ?? new OwinContext());

        /// <summary>
        /// Generates an <see cref="IOperationResult" /> for the specified <paramref name="ex" />.
        /// </summary>
        [NotNull]
        protected virtual IOperationResult CreateErrorResult([NotNull] Exception ex)
        {
            var operationResult = OperationResult.Error(ex);

            Exception unauthorized = ex as UnauthorizedAccessException;

            if (unauthorized != null)
            {
                operationResult = OperationResult.Unauthorized(ex.Message);
            }

            return operationResult;
        }

        /// <summary>
        /// Invokes the <paramref name="serviceTask" /> using an instance of <typeparamref name="TService" /> and returns the
        /// operation result.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="serviceTask">The specific task to be executed by the service.</param>
        /// <param name="service"></param>
        /// <returns></returns>
        [NotNull]
        protected virtual async Task<IOperationResult> ExcuteServiceTask<TService, TResult>([NotNull] Func<TService, Task<TResult>> serviceTask, [NotNull] TService service)
            where TResult : IOperationResult
        {
            if (serviceTask == null)
            {
                throw new ArgumentNullException(nameof(serviceTask));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            Contract.EndContractBlock();

            var operationResult = await serviceTask(service);

            if (operationResult == null)
            {
                throw new InvalidOperationException("The service task operation did not return a result.");
            }

            return operationResult;
        }

        /// <summary>
        /// Logging hook called when a service task throws an exception.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="result">The operation result, which is usually <c>null</c></param>
        protected abstract void LogException(Exception ex, [CanBeNull] IOperationResult result = null);

        /// <summary>
        /// Called after the service task has been completed and the response has been determined, but before the response has
        /// actually executed.
        /// </summary>
        /// <param name="httpResult">The HttpResponse created as a result of the operation.</param>
        /// <param name="result">The result data created by the operation.</param>
        protected virtual Task OnServiceTaskExecuted(IHttpActionResult httpResult, IOperationResult result)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Produces an <see cref="IHttpActionResult" /> based on the <see cref="OperationResultType" /> of the
        /// <see cref="IOperationResult" />.
        /// </summary>
        [NotNull]
        protected virtual IHttpActionResult OperationResponse([NotNull] IOperationResult result)
        {
            switch (result.Result)
            {
                case OperationResultType.Ok:

                    var fileContent = result.Data as FileContent;

                    if (fileContent != null)
                    {
                        if (fileContent.NotFound)
                        {
                            return this.NotFound();
                        }

                        return new FileContentResult(fileContent);
                    }

                    return this.Ok(result.Data ?? new MessageResult("ok"));

                case OperationResultType.NotFound:
                    if (!string.IsNullOrEmpty(result.Message))
                    {
                        return this.Content(HttpStatusCode.NotFound, result.Message);
                    }

                    return this.NotFound();

                case OperationResultType.Unauthorized:

                    if (!string.IsNullOrEmpty(result.Message))
                    {
                        return this.Content(HttpStatusCode.Unauthorized, result.Message);
                    }

                    return this.Unauthorized();

                case OperationResultType.Invalid:

                    if (this.Request.IsAjaxRequest())
                    {
                        var jsonResult = new JObject();

                        if (!string.IsNullOrWhiteSpace(result.Message))
                        {
                            jsonResult["message"] = result.Message;
                        }

                        if (result.ValidationResults != null)
                        {
                            jsonResult["errors"] = new JArray(
                                result.ValidationResults.Select(x => new JObject { ["fields"] = new JArray(x.MemberNames), ["message"] = x.ErrorMessage }));
                        }

                        return this.Content(HttpStatusCode.BadRequest, jsonResult);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(result.Message))
                        {
                            return this.Content(HttpStatusCode.BadRequest, result.Message);
                        }

                        return this.BadRequest();
                    }

                case OperationResultType.Error:
                    return this.Content(HttpStatusCode.BadRequest, result.Message);

                default:
                    throw new ArgumentOutOfRangeException(nameof(result.Result), result.Result, "Result type not supported.");
            }
        }

        /// <summary>
        /// Resolves the instance of <typeparamref name="TService" />.
        /// </summary>
        [NotNull]
        protected virtual TService ResolveService<TService>()
        {
            var svc = ServiceResolver.Resolve<TService>();

            if (svc == null)
            {
                throw new InvalidOperationException($"An instance of {nameof(TService)} could not be resoved.");
            }

            return svc;
        }

        /// <summary>
        /// Executes an operation task that does not return a value.
        /// </summary>
        /// <typeparam name="TService">The type of service being invoked.</typeparam>
        /// <paramref name="serviceTask">The specific service task to be executed: svc => svc.DeleteCustomer(5)</paramref>
        /// <returns>An Operation result indicating whether or not the operation succeeded, typically OperationResult.Ok()</returns>
        protected virtual Task<IHttpActionResult> ServiceTask<TService>(Func<TService, Task<OperationResult>> serviceTask)
        {
            return this.ServiceTaskImpl(serviceTask);
        }

        /// <summary>
        /// Executes an operation task that returns a value.
        /// </summary>
        /// <typeparam name="TService">The type of service being invoked.</typeparam>
        /// <typeparam name="TResult">The type of result generated by the service.</typeparam>
        /// <paramref name="serviceTask">The specific service task to be executed: svc => svc.GetCustomer(5)</paramref>
        /// <returns>
        /// An Operation result indicating whether or not the operation succeeded along with the data from the response,
        /// typically something like OperationResult.Ok(customer)
        /// </returns>
        protected virtual Task<IHttpActionResult> ServiceTask<TService, TResult>(Func<TService, Task<OperationResult<TResult>>> serviceTask)
        {
            return this.ServiceTaskImpl(serviceTask);
        }

        private async Task<IHttpActionResult> ServiceTaskImpl<TService, T>(Func<TService, Task<T>> serviceTask)
            where T : IOperationResult
        {
            IOperationResult result = null;

            try
            {
                var service = this.ResolveService<TService>();

                result = await this.ExcuteServiceTask(serviceTask, service);

                var httpResult = this.OperationResponse(result);

                await this.OnServiceTaskExecuted(httpResult, result);

                return httpResult;
            }
            catch (Exception ex)
            {
                try
                {
                    this.LogException(ex, result);
                }
                catch
                {
                    // ignored
                }

                var errorResult = this.CreateErrorResult(ex);

                return this.OperationResponse(errorResult);
            }
        }
    }
}
