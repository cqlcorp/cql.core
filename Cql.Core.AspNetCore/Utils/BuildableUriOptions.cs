using System;

namespace Cql.Core.AspNetCore.Utils
{
    [Flags]
    public enum BuildableUriOptions
    {
        None = 0x00,
        IncludePath = 0x01,
        IncludeQuery = 0x02,
        IncludePathAndQuery = IncludePath | IncludeQuery
    }
}
