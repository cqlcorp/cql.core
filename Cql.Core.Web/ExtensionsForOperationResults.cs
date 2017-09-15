namespace Cql.Core.Web
{
    using System.Threading.Tasks;

    public static class ExtensionsForOperationResults
    {
        /// <summary>
        /// Creates an <see cref="OperationResult" /> from the result of the specified <paramref name="task" />.
        /// </summary>
        public static Task<OperationResult<T>> AsOperationResult<T>(this Task<T> task)
        {
            return OperationResult.FromValue(task);
        }

        /// <summary>
        /// Converts an <see cref="IOperationResult" /> of one type to an <see cref="OperationResult" /> of
        /// <typeparam name="T"></typeparam>
        /// </summary>
        public static OperationResult<T> AsOperationResult<T>(this IOperationResult result, T data = default(T))
        {
            return OperationResult.FromOperationResult(result, data);
        }
    }
}
