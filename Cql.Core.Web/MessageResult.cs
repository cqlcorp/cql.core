// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="MessageResult.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using JetBrains.Annotations;

    /// <summary>
    /// The minimum implementation for a <seealso cref="IMessageResult"/>.
    /// </summary>
    /// <seealso cref="Cql.Core.Web.IMessageResult" />
    public class MessageResult : IMessageResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageResult" /> class.
        /// </summary>
        public MessageResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageResult" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageResult([CanBeNull] string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
    }
}
