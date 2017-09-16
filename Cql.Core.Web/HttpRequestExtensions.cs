// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="HttpRequestExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Web
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using JetBrains.Annotations;

    /// <summary>
    /// Class HttpRequestExtensions.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Returns <c>true</c> if the Accept header contains "application/json"; otherwise <c>false</c>.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if the request expects JSON, <c>false</c> otherwise.</returns>
        public static bool ExpectsJson([NotNull] this HttpRequestMessage request)
        {
            return request.Headers.Accept?.Any(x => x.MediaType == "application/json") == true;
        }

        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>true if the specified HTTP request is an AJAX request; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="request" /> parameter is null.</exception>
        public static bool IsAjaxRequest([NotNull] this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Headers.TryGetValues("X-Requested-With", out var headerValues)
                   && headerValues.Any(value => string.Equals(value, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase));
        }
    }
}
