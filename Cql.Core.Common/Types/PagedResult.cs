using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;

namespace Cql.Core.Common.Types
{
    public class PagedResult<T> : IPagerResult
    {
        private List<T> _results;
        private long? _totalPageCount;

        public PagedResult()
        {
        }

        public PagedResult(IPagingInfo paging)
        {
            if (paging == null)
            {
                return;
            }

            CurrentPageNumber = paging.PageNumber;
            PageSize = paging.PageSize;
        }

        public List<T> Results
        {
            [NotNull] get { return LazyInitializer.EnsureInitialized(ref _results, () => new List<T>()); }
            set { _results = value; }
        }

        /// <summary>
        /// The one-based page number
        /// </summary>
        public long CurrentPageNumber { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        public long PageSize { get; set; }

        /// <summary>
        /// The number of items in the current result set.
        /// </summary>
        public long ResultCount => Results.Count;

        /// <summary>
        /// The total number of pages in the full result set.
        /// </summary>
        public long TotalPageCount
        {
            get
            {
                if (_totalPageCount.HasValue)
                {
                    return _totalPageCount.Value;
                }

                return (long) Math.Ceiling(TotalRecords / (double) PageSize);
            }
            set { _totalPageCount = value; }
        }

        /// <summary>
        /// The total number of items in the full result set.
        /// </summary>
        public long TotalRecords { get; set; }
    }
}
