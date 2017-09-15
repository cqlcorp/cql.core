// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="CqlException.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Exceptions
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Class CqlException.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CqlException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CqlException" /> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        [StringFormatMethod("format")]
        public CqlException([NotNull] string format, [NotNull] params object[] args)
            : this(string.Format(format, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CqlException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CqlException([NotNull] string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CqlException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CqlException(string message, [NotNull] Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
