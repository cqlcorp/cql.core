// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="ExtensionsForClaimsIdentity.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;

    using JetBrains.Annotations;

    /// <summary>
    /// Class ExtensionsForClaimsIdentity.
    /// </summary>
    public static class ExtensionsForClaimsIdentity
    {
        /// <summary>
        /// Gets the role names.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns>An array containing the user's role names</returns>
        [NotNull]
        public static string[] GetRoleNames([CanBeNull] this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            return claimsIdentity?.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray() ?? new string[] { };
        }
    }
}
