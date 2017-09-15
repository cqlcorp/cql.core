// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="ExtensionsForIEnumerable.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    /// <summary>
    /// Class ExtensionsForIEnumerable.
    /// </summary>
    public static class ExtensionsForIEnumerable
    {
        /// <summary>
        /// Returns the current <paramref name="enumerable" /> instance as a Collection or enumerates the values to an Array.  
        /// <para> Useful for methods that possibly enumerate a collection more than once.</para>
        /// </summary>
        /// <typeparam name="T">The type of collection</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>A collection instance</returns>
        [NotNull]
        public static IReadOnlyCollection<T> AsReadOnlyCollection<T>([CanBeNull] this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return new T[] { };
            }

            return enumerable as IReadOnlyCollection<T> ?? enumerable.ToArray();
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the default equality comparer for the projected type.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>
        /// <remarks>This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.</remarks>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>([NotNull] this IEnumerable<TSource> source, [NotNull] Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the specified comparer for the projected type.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>
        /// <exception cref="ArgumentNullException">source
        /// or
        /// keySelector</exception>
        /// <remarks>This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.</remarks>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>([NotNull] this IEnumerable<TSource> source, [NotNull] Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
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

        /// <summary>
        /// Partitions the specified size.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="size">The size.</param>
        /// <returns>IEnumerable&lt;IEnumerable&lt;T&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">sequence</exception>
        public static IEnumerable<IEnumerable<T>> Partition<T>([NotNull] this IEnumerable<T> sequence, int size)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            var partition = new List<T>(size);

            foreach (var obj in sequence)
            {
                partition.Add(obj);

                if (partition.Count != size)
                {
                    continue;
                }

                yield return partition;

                partition = new List<T>(size);
            }

            if (partition.Count > 0)
            {
                yield return partition;
            }
        }

        /// <summary>
        /// Selects to array.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>TResult[].</returns>
        [NotNull]
        [MustUseReturnValue]
        public static TResult[] SelectToArray<TSource, TResult>([NotNull] this IEnumerable<TSource> collection, [NotNull] Func<TSource, TResult> selector)
        {
            return collection.Select(selector).ToArray();
        }

        /// <summary>
        /// Selects to list.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>List&lt;TResult&gt;.</returns>
        [NotNull]
        [MustUseReturnValue]
        public static List<TResult> SelectToList<TSource, TResult>([NotNull] this IEnumerable<TSource> collection, [NotNull] Func<TSource, TResult> selector)
        {
            return collection.Select(selector).ToList();
        }

        /// <summary>
        /// To the i list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        [NotNull]
        public static IList<T> ToIList<T>([CanBeNull] this IEnumerable<T> enumerable)
        {
            return enumerable as IList<T> ?? enumerable?.ToList() ?? new List<T>();
        }

        /// <summary>
        /// Distincts the by implementation.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>IEnumerable&lt;TSource&gt;.</returns>
        private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
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
