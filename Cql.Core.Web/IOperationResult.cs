using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cql.Core.Web
{
    public interface IOperationResult : IMessageResult
    {
        object Data { get; }

        OperationResultType Result { get; }

        IEnumerable<ValidationResult> ValidationResults { get; }
    }
}
