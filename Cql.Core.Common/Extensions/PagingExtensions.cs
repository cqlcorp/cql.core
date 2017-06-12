using System;
using System.Collections.Generic;
using System.Linq;
using Cql.Core.Common.Types;

namespace Cql.Core.Common.Extensions
{
    public static class PagingExtensions
    {
        public static IEnumerable<T> SkipToPage<T>(this IEnumerable<T> list, IPagingInfo pagingInfo)
        {
            if (list == null)
            {
                return new List<T>();
            }

            if (pagingInfo == null)
            {
                pagingInfo = new PagingInfo();
            }

            return list
                .Skip(Convert.ToInt32((pagingInfo.PageNumber - 1) * pagingInfo.PageSize))
                .Take(Convert.ToInt32(pagingInfo.PageSize));
        }

        public static PagedResult<TDest> Transform<TSource, TDest>(this PagedResult<TSource> result, Func<TSource, TDest> selector)
        {
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
