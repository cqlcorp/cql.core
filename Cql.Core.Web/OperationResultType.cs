// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="OperationResultType.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    /// <summary>
    /// Enum OperationResultType
    /// </summary>
    public enum OperationResultType
    {
        /// <summary>
        /// Indicates success
        /// </summary>
        Ok = 0,

        /// <summary>
        /// Indicates that the result is null or not found.
        /// </summary>
        NotFound = 1,

        /// <summary>
        /// Indicates that the result could not complete because of an permissions issue.
        /// </summary>
        Unauthorized = 2,

        /// <summary>
        /// Indicates that the result could not complete because of validation issues.
        /// </summary>
        Invalid = 3,

        /// <summary>
        /// Indicates that the result could not complete because of an error.
        /// </summary>
        Error = 4
    }
}
