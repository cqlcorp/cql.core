namespace Cql.Core.Web
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public interface IOperationResult : IMessageResult
    {
        object Data { get; }

        OperationResultType Result { get; }

        IEnumerable<ValidationResult> ValidationResults { get; }
    }
}
