namespace Cql.Core.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Returns <c>true</c> if the Accept header contains "application/json"; otherwise <c>false</c>.
        /// </summary>
        public static bool ExpectsJson(this HttpRequestMessage request)
        {
            return request.Headers.Accept?.Any(x => x.MediaType == "application/json") == true;
        }

        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// <returns>
        /// true if the specified HTTP request is an AJAX request; otherwise, false.
        /// </returns>
        /// <param name="request">The HTTP request.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="request" /> parameter is null (Nothing in Visual
        /// Basic).
        /// </exception>
        public static bool IsAjaxRequest(this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            IEnumerable<string> headerValues;

            return request.Headers.TryGetValues("X-Requested-With", out headerValues)
                   && headerValues.Any(value => string.Equals(value, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase));
        }
    }
}
