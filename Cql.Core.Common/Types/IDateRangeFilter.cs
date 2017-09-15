namespace Cql.Core.Common.Types
{
    using System;

    public interface IDateRangeFilter
    {
        DateTime? DateFrom { get; set; }

        DateTime? DateTo { get; set; }
    }
}
