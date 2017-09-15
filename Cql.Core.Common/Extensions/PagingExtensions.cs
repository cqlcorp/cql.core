// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="PagingExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Cql.Core.Common.Types;

    using JetBrains.Annotations;

    /// <summary>
    /// Class PagingExtensions.
    /// </summary>
    public static class PagingExtensions
    {
        /// <summary>
        /// Performs a Skip/Take operation on the current list.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="pagingInfo">The paging information.</param>
        /// <returns>An IEnumerable&lt;T&gt;.</returns>
        [NotNull]
        public static IEnumerable<T> SkipToPage<T>([CanBeNull] this IEnumerable<T> list, [CanBeNull] IPagingInfo pagingInfo)
        {
            if (list == null)
            {
                return new List<T>();
            }

            if (pagingInfo == null)
            {
                pagingInfo = new PagingInfo();
            }

            return list.Skip(Convert.ToInt32((pagingInfo.PageNumber - 1) * pagingInfo.PageSize)).Take(Convert.ToInt32(pagingInfo.PageSize));
        }

#if !NETSTANDARD1_6 
        /// <summary>
        /// Performs a Skip/Take operation on the current list.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="pagingInfo">The paging information.</param>
        /// <returns>An IQueryable&lt;T&gt;.</returns>
        [NotNull]
        public static IQueryable<T> SkipToPage<T>([CanBeNull] this IQueryable<T> list, [CanBeNull] IPagingInfo pagingInfo)
        {
            if (list == null)
            {
                return Enumerable.Empty<T>().AsQueryable();
            }

            if (pagingInfo == null)
            {
                pagingInfo = new PagingInfo();
            }

            return list.Skip(Convert.ToInt32((pagingInfo.PageNumber - 1) * pagingInfo.PageSize)).Take(Convert.ToInt32(pagingInfo.PageSize));
        }
#endif

        /// <summary>
        /// Transforms the specified selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TDest">The type of the t dest.</typeparam>
        /// <param name="result">The result.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>A PagedResult&lt;TDest&gt;.</returns>
        /// <exception cref="ArgumentNullException">The result cannot be null</exception>
        [NotNull]
        public static PagedResult<TDest> Transform<TSource, TDest>([NotNull] this PagedResult<TSource> result, [NotNull] Func<TSource, TDest> selector)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return new PagedResult<TDest>
                       {
                           Results = result.Results.SelectToList(selector),
                           PageSize = result.PageSize,
                           TotalRecords = result.TotalRecords,
                           CurrentPageNumber = result.CurrentPageNumber
                       };
        }
    }
}
