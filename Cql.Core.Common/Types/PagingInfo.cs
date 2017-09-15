namespace Cql.Core.Common.Types
{
    public class PagingInfo : IPagingInfo
    {
        public static long DefaultPageSize = 20;

        private long _pageNumber;

        private long _pageSize;

        public PagingInfo()
        {
            this.PageNumber = 1;
            this.PageSize = DefaultPageSize;
        }

        public PagingInfo(long pageNumber, long pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// The one-based page number.
        /// </summary>
        public long PageNumber
        {
            get
            {
                if (this._pageNumber < 1)
                {
                    this._pageNumber = 1;
                }

                return this._pageNumber;
            }

            set => this._pageNumber = value;
        }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        public long PageSize
        {
            get
            {
                if (this._pageSize < 1)
                {
                    this._pageSize = 1;
                }

                return this._pageSize;
            }

            set => this._pageSize = value;
        }
    }
}
