using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Cql.Core.Common.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Awaits the current task and returns the result as a list.
        /// </summary>
        [NotNull]
        public static async Task<List<T>> ToList<T>(this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable as List<T> ?? enumerable?.ToList() ?? new List<T>();
        }

        /// <summary>
        /// Awaits the current task and returns a single result.
        /// </summary>
        [NotNull]
        public static async Task<T> Single<T>(this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable.Single();
        }

        /// <summary>
        /// Awaits the current task and returns a single result or null.
        /// </summary>
        [NotNull]
        public static async Task<T> SingleOrDefault<T>(this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable == null ? default(T) : enumerable.SingleOrDefault();
        }

        /// <summary>
        /// Awaits the current task and returns the first result.
        /// </summary>
        [NotNull]
        public static async Task<T> First<T>(this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable.First();
        }

        /// <summary>
        /// Awaits the current task and returns the first result or null.
        /// </summary>
        [NotNull]
        public static async Task<T> FirstOrDefault<T>(this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable == null ? default(T) : enumerable.FirstOrDefault();
        }

        /// <summary>
        /// Awaits the current task and returns the result as an array.
        /// </summary>
        [NotNull]
        public static async Task<T[]> ToArray<T>(this Task<IEnumerable<T>> enumerableTask)
        {
            var enumerable = await enumerableTask;
            return enumerable as T[] ?? enumerable?.ToArray() ?? new T[] {};
        }

        /// <summary>
        /// Wraps the current IEnumerable as a completed Task that returns a List
        /// </summary>
        public static Task<List<T>> ToListAsync<T>(this IEnumerable<T> collection)
        {
            return collection == null ? Task.FromResult(default(List<T>)) : collection.ToList().ToAsync();
        }

        /// <summary>
        /// Wraps the current IEnumerable as a completed Task that returns an array
        /// </summary>
        public static Task<T[]> ToArrayAsync<T>(this IEnumerable<T> collection)
        {
            return collection == null ? Task.FromResult(default(T[])) : collection.ToArray().ToAsync();
        }

        /// <summary>
        /// Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that's completed successfully with the specified
        /// result.
        /// </summary>
        /// <returns>The successfully completed task.</returns>
        /// <param name="result">The result to store into the completed task. </param>
        /// <typeparam name="TResult">The type of the result returned by the task. </typeparam>
        public static Task<TResult> ToAsync<TResult>(this TResult result)
        {
            return Task.FromResult(result);
        }
    }
}
