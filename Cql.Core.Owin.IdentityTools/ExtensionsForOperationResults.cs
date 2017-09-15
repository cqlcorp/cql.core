namespace Cql.Core.Owin.IdentityTools
{
    using System.Linq;

    using Cql.Core.Web;

    using Microsoft.AspNet.Identity;

    public static class ExtensionsForOperationResults
    {
        public static OperationResult AsOperationResult(this IdentityResult result)
        {
            return FromIdentityResult(result);
        }

        public static OperationResult<TResult> AsOperationResult<TResult>(this IdentityResult result)
        {
            return FromIdentityResult(result).AsOperationResult<TResult>();
        }

        public static OperationResult FromIdentityResult(IdentityResult result)
        {
            return result?.Succeeded == true ? OperationResult.Ok() : OperationResult.Error(string.Join(", ", result?.Errors ?? Enumerable.Empty<string>()));
        }
    }
}
