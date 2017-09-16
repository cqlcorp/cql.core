// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="IOperationResult.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using JetBrains.Annotations;

    /// <summary>
    /// The base contract for an <see cref="OperationResult"/>.
    /// </summary>
    /// <seealso cref="Cql.Core.Web.IMessageResult" />
    public interface IOperationResult : IMessageResult
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        [CanBeNull]
        object Data { get; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The result.</value>
        OperationResultType Result { get; }

        /// <summary>
        /// Gets the validation results.
        /// </summary>
        /// <value>The validation results.</value>
        [CanBeNull]
        IEnumerable<ValidationResult> ValidationResults { get; }
    }
}
