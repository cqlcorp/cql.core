// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="TaskExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    /// Class TaskExtensions.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Awaits the current task and returns the first result.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="enumerableTask">The enumerable task.</param>
        /// <returns>The first instance of T</returns>
        public static async Task<T> First<T>([NotNull] this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable.First();
        }

        /// <summary>
        /// Awaits the current task and returns the first result or null.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="enumerableTask">The enumerable task.</param>
        /// <returns>The first instance of T or its default</returns>
        public static async Task<T> FirstOrDefault<T>([NotNull] this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable == null ? default(T) : enumerable.FirstOrDefault();
        }

        /// <summary>
        /// Awaits the current task and returns a single result.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="enumerableTask">The enumerable task.</param>
        /// <returns>A single instance of T.</returns>
        public static async Task<T> Single<T>([NotNull] this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable.Single();
        }

        /// <summary>
        /// Awaits the current task and returns a single result or null.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="enumerableTask">The enumerable task.</param>
        /// <returns>The single instance of T or its default</returns>
        public static async Task<T> SingleOrDefault<T>([NotNull] this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable == null ? default(T) : enumerable.SingleOrDefault();
        }

        /// <summary>
        /// Awaits the current task and returns the result as an array.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="enumerableTask">The enumerable task.</param>
        /// <returns>A Task&lt;T[]&gt;.</returns>
        [ItemNotNull]
        public static async Task<T[]> ToArray<T>([NotNull] this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable as T[] ?? enumerable?.ToArray() ?? new T[] { };
        }

        /// <summary>
        /// Wraps the current IEnumerable as a completed Task that returns an array
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>A Task&lt;T[]&gt;.</returns>
        public static Task<T[]> ToArrayAsync<T>([CanBeNull] this IEnumerable<T> collection)
        {
            return collection == null ? Task.FromResult(default(T[])) : collection.ToArray().ToAsync();
        }

        /// <summary>
        /// Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that's completed successfully with the specified
        /// result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the task.</typeparam>
        /// <param name="result">The result to store into the completed task.</param>
        /// <returns>The successfully completed task.</returns>
        public static Task<TResult> ToAsync<TResult>(this TResult result)
        {
            return Task.FromResult(result);
        }

        /// <summary>
        /// Awaits the current task and returns the result as a list.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="enumerableTask">The enumerable task.</param>
        /// <returns>A Task&lt;List&lt;T&gt;&gt;.</returns>
        public static async Task<List<T>> ToList<T>([NotNull] this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable as List<T> ?? enumerable?.ToList() ?? new List<T>();
        }

        /// <summary>
        /// Wraps the current IEnumerable as a completed Task that returns a List
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>A Task&lt;List&lt;T&gt;&gt;.</returns>
        public static Task<List<T>> ToListAsync<T>([CanBeNull] this IEnumerable<T> collection)
        {
            return collection == null ? Task.FromResult(default(List<T>)) : collection.ToList().ToAsync();
        }
    }
}
