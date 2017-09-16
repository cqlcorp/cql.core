// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-16-2017
// ***********************************************************************
// <copyright file="IMessageResult.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using JetBrains.Annotations;

    /// <summary>
    /// An abstract interface for a result type that contains a <see cref="Message" /> property.
    /// </summary>
    public interface IMessageResult
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [CanBeNull]
        string Message { get; set; }
    }
}
