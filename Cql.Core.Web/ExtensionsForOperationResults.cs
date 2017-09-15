// ***********************************************************************
// Assembly         : Cql.Core.Web
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="ExtensionsForOperationResults.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Web
{
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    /// Class ExtensionsForOperationResults.
    /// </summary>
    public static class ExtensionsForOperationResults
    {
        /// <summary>
        /// Creates an <see cref="OperationResult" /> from the result of the specified <paramref name="task" />.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="task">The task.</param>
        /// <returns>A Task&lt;OperationResult&lt;T&gt;&gt;.</returns>
        [NotNull]
        public static Task<OperationResult<T>> AsOperationResult<T>([NotNull] this Task<T> task)
        {
            return OperationResult.FromValue(task);
        }

        /// <summary>
        /// Converts an <see cref="IOperationResult" /> of one type to an <see cref="OperationResult" /> of
        /// <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="result">The result.</param>
        /// <param name="data">The data.</param>
        /// <returns>An OperationResult&lt;T&gt;.</returns>
        [NotNull]
        public static OperationResult<T> AsOperationResult<T>([NotNull] this IOperationResult result, [CanBeNull] T data = default(T))
        {
            return OperationResult.FromOperationResult(result, data);
        }
    }
}
