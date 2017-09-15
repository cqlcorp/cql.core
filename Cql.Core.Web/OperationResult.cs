// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="OperationResult.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    /// Wraps the result of a service call with information indicating success or failure of the operation.
    /// </summary>
    /// <seealso cref="Cql.Core.Web.IOperationResult" />
    public class OperationResult : IOperationResult
    {
        /// <summary>
        /// Gets or sets the default error message.
        /// </summary>
        /// <value>The default error message.</value>
        [NotNull]
        public static string DefaultErrorMessage { get; set; } = "An error occured.";

        /// <summary>
        /// Gets or sets the default validation message.
        /// </summary>
        /// <value>The default validation message.</value>
        [NotNull]
        public static string DefaultValidationMessage { get; set; } = "The model is not valid.";

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public OperationResultType Result { get; set; }

        /// <summary>
        /// Gets or sets the validation results.
        /// </summary>
        /// <value>The validation results.</value>
        public IEnumerable<ValidationResult> ValidationResults { get; set; }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating failure with an error message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>An OperationResult.</returns>
        [NotNull]
        public static OperationResult Error([CanBeNull] Exception ex = null)
        {
            return Error(ex?.Message ?? DefaultErrorMessage);
        }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating failure with an error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>An OperationResult.</returns>
        [NotNull]
        public static OperationResult Error([CanBeNull] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = DefaultErrorMessage;
            }

            return new OperationResult { Message = message, Result = OperationResultType.Error };
        }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating failure with an error message.
        /// </summary>
        /// <typeparam name="T">The operation result type</typeparam>
        /// <param name="ex">The exception.</param>
        /// <returns>An OperationResult&lt;T&gt;.</returns>
        [NotNull]
        public static OperationResult<T> Error<T>([CanBeNull] Exception ex = null)
        {
            return Error<T>(ex?.Message ?? DefaultErrorMessage);
        }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating failure with an error message.
        /// </summary>
        /// <typeparam name="T">The operation result type</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>An OperationResult&lt;T&gt;.</returns>
        [NotNull]
        public static OperationResult<T> Error<T>([CanBeNull] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = DefaultErrorMessage;
            }

            return new OperationResult<T> { Message = message, Result = OperationResultType.Error };
        }

        /// <summary>
        /// Creates a new operation result with the return type of <typeparamref name="T" /> with all of the same values.
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="other">The other operation.</param>
        /// <param name="data">The result data (optional).</param>
        /// <returns>A OperationResult&lt;T&gt;.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="other" /> cannot be null</exception>
        [NotNull]
        public static OperationResult<T> FromOperationResult<T>([NotNull] IOperationResult other, [CanBeNull] T data = default(T))
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return new OperationResult<T> { Message = other.Message, Result = other.Result, ValidationResults = other.ValidationResults, Data = data };
        }

        /// <summary>
        /// Returns a NotFound result if they specified <paramref name="value" /> is NULL; otherwise an OK result is returned.
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>An OperationResult&lt;T&gt;.</returns>
        [NotNull]
        public static OperationResult<T> FromValue<T>([CanBeNull] T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T)) ? NotFound<T>("No results") : Ok(value);
        }

        /// <summary>
        /// Returns a NotFound result if the result of the specified <paramref name="valueTask" /> is NULL; otherwise an OK
        /// result is returned.
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="valueTask">The value task.</param>
        /// <returns>An operation result of type <typeparamref name="T" /></returns>
        /// <exception cref="ArgumentNullException">The <paramref name="valueTask" /> cannot be null</exception>
        [ItemNotNull]
        public static async Task<OperationResult<T>> FromValue<T>([NotNull] Task<T> valueTask)
        {
            if (valueTask == null)
            {
                throw new ArgumentNullException(nameof(valueTask));
            }

            return FromValue(await valueTask);
        }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating validation errors with the specified
        /// <paramref name="validationResults" />.
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="validationResults">The validation results.</param>
        /// <returns>An OperationResult&lt;T&gt;.</returns>
        [NotNull]
        public static OperationResult<T> Invalid<T>([CanBeNull] IEnumerable<ValidationResult> validationResults = null)
        {
            return new OperationResult<T>
                       {
                           ValidationResults = validationResults ?? new List<ValidationResult> { new ValidationResult(DefaultValidationMessage) },
                           Result = OperationResultType.Invalid
                       };
        }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating validation errors with the specified
        /// <paramref name="validationResults" />.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        /// <returns>An OperationResult.</returns>
        [NotNull]
        public static OperationResult Invalid([CanBeNull] IEnumerable<ValidationResult> validationResults = null)
        {
            return new OperationResult
                       {
                           ValidationResults = validationResults ?? new List<ValidationResult> { new ValidationResult(DefaultValidationMessage) },
                           Result = OperationResultType.Invalid
                       };
        }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating that the operation resulted in a null or not found result.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>An OperationResult.</returns>
        [NotNull]
        public static OperationResult NotFound([CanBeNull] string message = null)
        {
            return new OperationResult { Message = message, Result = OperationResultType.NotFound };
        }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating that the operation resulted in a null or not found result.
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>An OperationResult&lt;T&gt;.</returns>
        [NotNull]
        public static OperationResult<T> NotFound<T>([CanBeNull] string message = null)
        {
            return new OperationResult<T> { Message = message, Result = OperationResultType.NotFound };
        }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating that the operation was successful.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>An OperationResult.</returns>
        [NotNull]
        public static OperationResult Ok([CanBeNull] object data = null)
        {
            return new OperationResult { Data = data, Result = OperationResultType.Ok };
        }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating that the operation was successful.
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="data">The data.</param>
        /// <returns>An OperationResult.</returns>
        [NotNull]
        public static OperationResult<T> Ok<T>([CanBeNull] T data = default(T))
        {
            return new OperationResult<T> { Data = data, Result = OperationResultType.Ok };
        }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating unauthorized access with an error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>An OperationResult&lt;T&gt;.</returns>
        [NotNull]
        public static OperationResult Unauthorized([CanBeNull] string message = null)
        {
            return new OperationResult { Message = message, Result = OperationResultType.Unauthorized };
        }

        /// <summary>
        /// Returns an <see cref="OperationResult" /> indicating unauthorized access with an error message.
        /// </summary>
        /// <typeparam name="T">The operation result type</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>An OperationResult&lt;T&gt;.</returns>
        [NotNull]
        public static OperationResult<T> Unauthorized<T>([CanBeNull] string message = null)
        {
            return new OperationResult<T> { Message = message, Result = OperationResultType.Unauthorized };
        }
    }

    /// <summary>
    /// Wraps the result of a service call with a strongly typed result containing information indicating success or failure of
    /// the operation.
    /// </summary>
    /// <typeparam name="T">The strongly typed result</typeparam>
    /// <seealso cref="Cql.Core.Web.OperationResult" />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class OperationResult<T> : OperationResult
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public new T Data
        {
            get => (T)base.Data;
            set => base.Data = value;
        }
    }
}
