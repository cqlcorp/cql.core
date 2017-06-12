namespace Cql.Core.Common.Types
{
    public interface IPagerResult
    {
        /// <summary>
        /// The one-based page number
        /// </summary>
        long CurrentPageNumber { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        long PageSize { get; set; }

        /// <summary>
        /// The number of items in the current result set.
        /// </summary>
        long ResultCount { get; }

        /// <summary>
        /// The total number of pages in the full result set.
        /// </summary>
        long TotalPageCount { get; set; }

        /// <summary>
        /// The total number of items in the full result set.
        /// </summary>
        long TotalRecords { get; set; }
    }
}
