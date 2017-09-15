using Cql.Core.Web;

namespace Cql.Core.AspNetCore
{
    public class RedirectToUrlOperationResult : OperationResult
    {
        public RedirectToUrlOperationResult(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
    }
}
