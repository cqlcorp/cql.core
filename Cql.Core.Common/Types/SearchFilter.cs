using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cql.Core.Common.Types
{
    public class SearchFilter<TOrderBy> : PagingInfo where TOrderBy : struct
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public TOrderBy? OrderBy { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SortOrder? SortOrder { get; set; }
    }
}
