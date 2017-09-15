namespace Cql.Core.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    public class OperationResult : IOperationResult
    {
        public static string DefaultErrorMessage = "An error occured.";

        public static string DefaultValidationMessage { get; set; } = "The model is not valid.";

        public object Data { get; set; }

        public string Message { get; set; }

        public OperationResultType Result { get; set; }

        public IEnumerable<ValidationResult> ValidationResults { get; set; }

        public static OperationResult Error(Exception ex)
        {
            return Error(ex?.Message ?? DefaultErrorMessage);
        }

        public static OperationResult Error(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = DefaultErrorMessage;
            }

            return new OperationResult { Message = message, Result = OperationResultType.Error };
        }

        public static OperationResult<T> Error<T>(Exception ex)
        {
            return Error<T>(ex.Message);
        }

        public static OperationResult<T> Error<T>(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = DefaultErrorMessage;
            }

            return new OperationResult<T> { Message = message, Result = OperationResultType.Error };
        }

        public static OperationResult<T> FromOperationResult<T>(IOperationResult other, T data = default(T))
        {
            return new OperationResult<T> { Message = other.Message, Result = other.Result, ValidationResults = other.ValidationResults, Data = data };
        }

        /// <summary>
        /// Returns a NotFound result if they specified <paramref name="value" /> is NULL; otherwise an OK result is returned.
        /// </summary>
        public static OperationResult<T> FromValue<T>(T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T)) ? NotFound<T>("No results") : Ok(value);
        }

        /// <summary>
        /// Returns a NotFound result if the result of the specified <paramref name="valueTask" /> is NULL; otherwise an OK
        /// result is returned.
        /// </summary>
        public static async Task<OperationResult<T>> FromValue<T>(Task<T> valueTask)
        {
            return FromValue(await valueTask);
        }

        public static OperationResult<T> Invalid<T>(IEnumerable<ValidationResult> validationResults)
        {
            return new OperationResult<T>
                       {
                           ValidationResults = validationResults ?? new List<ValidationResult> { new ValidationResult(DefaultValidationMessage) },
                           Result = OperationResultType.Invalid
                       };
        }

        public static OperationResult Invalid(IEnumerable<ValidationResult> validationResults)
        {
            return new OperationResult
                       {
                           ValidationResults = validationResults ?? new List<ValidationResult> { new ValidationResult(DefaultValidationMessage) },
                           Result = OperationResultType.Invalid
                       };
        }

        public static OperationResult NotFound(string message = null)
        {
            return new OperationResult { Message = message, Result = OperationResultType.NotFound };
        }

        public static OperationResult<T> NotFound<T>(string message = null)
        {
            return new OperationResult<T> { Message = message, Result = OperationResultType.NotFound };
        }

        public static OperationResult Ok(object data = null)
        {
            return new OperationResult { Data = data, Result = OperationResultType.Ok };
        }

        public static OperationResult<T> Ok<T>()
        {
            return new OperationResult<T> { Result = OperationResultType.Ok };
        }

        public static OperationResult<T> Ok<T>(T data)
        {
            return new OperationResult<T> { Data = data, Result = OperationResultType.Ok };
        }

        public static OperationResult Unauthorized(string message = null)
        {
            return new OperationResult { Message = message, Result = OperationResultType.Unauthorized };
        }

        public static OperationResult<T> Unauthorized<T>(string message = null)
        {
            return new OperationResult<T> { Message = message, Result = OperationResultType.Unauthorized };
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public new T Data
        {
            get => (T)base.Data;
            set => base.Data = value;
        }
    }
}
