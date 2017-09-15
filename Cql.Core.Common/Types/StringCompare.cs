// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="StringCompare.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Types
{
    /// <summary>
    /// The string compare options.
    /// </summary>
    public enum StringCompare
    {
        /// <summary>
        /// The values are compared using equality comparison.
        /// </summary>
        Equals = 0,

        /// <summary>
        /// The value starts with supplied criteria.
        /// </summary>
        StartsWith = 1,

        /// <summary>
        /// The value contains the supplied criteria.
        /// </summary>
        Contains = 2,

        /// <summary>
        /// The value ends with supplied criteria.
        /// </summary>
        EndsWith = 3
    }
}
