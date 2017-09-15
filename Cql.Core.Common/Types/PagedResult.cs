// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-15-2017
// ***********************************************************************
// <copyright file="PagedResult.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Types
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using JetBrains.Annotations;

    /// <summary>
    /// Class PagedResult.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Cql.Core.Common.Types.IPagerResult" />
    /// <seealso cref="Cql.Core.Common.Types.IPagerResult" />
    public class PagedResult<T> : IPagerResult
    {
        /// <summary>
        /// The results
        /// </summary>
        private List<T> _results;

        /// <summary>
        /// The total page count
        /// </summary>
        private long? _totalPageCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{T}" /> class.
        /// </summary>
        public PagedResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{T}" /> class.
        /// </summary>
        /// <param name="paging">The paging.</param>
        public PagedResult(IPagingInfo paging)
        {
            if (paging == null)
            {
                return;
            }

            this.CurrentPageNumber = paging.PageNumber;
            this.PageSize = paging.PageSize;
        }

        /// <summary>
        /// The one-based page number
        /// </summary>
        /// <value>The current page number.</value>
        public long CurrentPageNumber { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        /// <value>The size of the page.</value>
        public long PageSize { get; set; }

        /// <summary>
        /// The number of items in the current result set.
        /// </summary>
        /// <value>The result count.</value>
        public long ResultCount => this.Results.Count;

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        public List<T> Results
        {
            [NotNull]
            get
            {
                return LazyInitializer.EnsureInitialized(ref this._results, () => new List<T>());
            }

            set
            {
                this._results = value;
            }
        }

        /// <summary>
        /// The total number of pages in the full result set.
        /// </summary>
        /// <value>The total page count.</value>
        public long TotalPageCount
        {
            get
            {
                if (this._totalPageCount.HasValue)
                {
                    return this._totalPageCount.Value;
                }

                return (long)Math.Ceiling(this.TotalRecords / (double)this.PageSize);
            }

            set => this._totalPageCount = value;
        }

        /// <summary>
        /// The total number of items in the full result set.
        /// </summary>
        /// <value>The total records.</value>
        public long TotalRecords { get; set; }
    }
}
