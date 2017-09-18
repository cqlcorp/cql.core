// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="ExtensionsForSqlExceptions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Data.SqlClient;

    using Cql.Core.Common.Extensions;

    using JetBrains.Annotations;

    /// <summary>
    /// Class ExtensionsForSqlExceptions.
    /// </summary>
    public static class ExtensionsForSqlExceptions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="ex"/> indicates an Unique Key constraint violation.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns><c>true</c> if the specified <paramref name="ex"/> indicates an Unique Key constraint violation; otherwise, <c>false</c>.</returns>
        public static bool IsUniqueKeyConstraintViolation([NotNull] this SqlException ex)
        {
            return ex.Message.Contains("Violation of UNIQUE KEY constraint", StringComparison.OrdinalIgnoreCase);
        }
    }
}
