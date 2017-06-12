namespace Cql.Core.Common.Types
{
    public class PagingInfo : IPagingInfo
    {
        public static long DefaultPageSize = 20;

        private long _pageNumber;
        private long _pageSize;

        public PagingInfo()
        {
            PageNumber = 1;
            PageSize = DefaultPageSize;
        }

        public PagingInfo(long pageNumber, long pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        /// <summary>
        /// The one-based page number.
        /// </summary>
        public long PageNumber
        {
            get
            {
                if (_pageNumber < 1)
                {
                    _pageNumber = 1;
                }
                return _pageNumber;
            }
            set { _pageNumber = value; }
        }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        public long PageSize
        {
            get
            {
                if (_pageSize < 1)
                {
                    _pageSize = 1;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }
    }
}
