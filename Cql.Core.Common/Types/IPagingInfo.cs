namespace Cql.Core.Common.Types
{
    public interface IPagingInfo
    {
        /// <summary>
        /// The one-based page number.
        /// </summary>
        long PageNumber { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        long PageSize { get; set; }
    }
}
