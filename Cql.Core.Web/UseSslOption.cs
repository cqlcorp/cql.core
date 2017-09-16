// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="UseSslOption.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    /// <summary>
    /// Options for determining which protocol is used when constructing an absolute URL.
    /// </summary>
    public enum UseSslOption
    {
        /// <summary>
        /// Generates a link that matches the current request.
        /// </summary>
        UseCurrentRequest = 0,

        /// <summary>
        /// Generates a link that always uses HTTP traffic.
        /// </summary>
        Never = 1,

        /// <summary>
        /// Generates a link that always uses SSL traffic.
        /// </summary>
        Always = 2
    }
}
