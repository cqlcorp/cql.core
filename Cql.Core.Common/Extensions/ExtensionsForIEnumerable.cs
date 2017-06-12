using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Cql.Core.Common.Extensions
{
    public static class ExtensionsForIEnumerable
    {
        public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return new T[] {};
            }

            return enumerable as IReadOnlyCollection<T> ?? enumerable.ToArray();
        }

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> sequence, int size)
        {
            var partition = new List<T>(size);

            foreach (var obj in sequence)
            {
                partition.Add(obj);
                if (partition.Count == size)
                {
                    yield return partition;

                    partition = new List<T>(size);
                }
            }

            if (partition.Count > 0)
            {
                yield return partition;
            }
        }

        public static List<TResult> SelectToList<TSource, TResult>(this IEnumerable<TSource> collection, Func<TSource, TResult> selector)
        {
            return collection.Select(selector).ToList();
        }

        public static TResult[] SelectToArray<TSource, TResult>(this IEnumerable<TSource> collection, Func<TSource, TResult> selector)
        {
            return collection.Select(selector).ToArray();
        }

        [NotNull]
        public static IList<T> ToIList<T>(this IEnumerable<T> enumerable)
        {
            return enumerable as IList<T> ?? enumerable?.ToList() ?? new List<T>();
        }


        /// <summary>
        ///     Returns all distinct elements of the given source, where "distinctness"
        ///     is determined via a projection and the default equality comparer for the projected type.
        /// </summary>
        /// <remarks>
        ///     This operator uses deferred execution and streams the results, although
        ///     a set of already-seen keys is retained. If a key is seen multiple times,
        ///     only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <returns>
        ///     A sequence consisting of distinct elements from the source sequence,
        ///     comparing them by the specified key projection.
        /// </returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        /// <summary>
        ///     Returns all distinct elements of the given source, where "distinctness"
        ///     is determined via a projection and the specified comparer for the projected type.
        /// </summary>
        /// <remarks>
        ///     This operator uses deferred execution and streams the results, although
        ///     a set of already-seen keys is retained. If a key is seen multiple times,
        ///     only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <param name="comparer">
        ///     The equality comparer to use to determine whether or not keys are equal.
        ///     If null, the default equality comparer for <c>TSource</c> is used.
        /// </param>
        /// <returns>
        ///     A sequence consisting of distinct elements from the source sequence,
        ///     comparing them by the specified key projection.
        /// </returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return DistinctByImpl(source, keySelector, comparer);
        }

        private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(
            IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            var knownKeys = new HashSet<TKey>(comparer);
            foreach (var element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
