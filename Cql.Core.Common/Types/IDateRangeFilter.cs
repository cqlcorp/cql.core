using System;

namespace Cql.Core.Common.Types
{
    public interface IDateRangeFilter
    {
        DateTime? DateFrom { get; set; }

        DateTime? DateTo { get; set; }
    }
}
