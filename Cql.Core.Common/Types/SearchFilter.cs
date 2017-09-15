namespace Cql.Core.Common.Types
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class SearchFilter<TOrderBy> : PagingInfo
        where TOrderBy : struct
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public TOrderBy? OrderBy { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SortOrder? SortOrder { get; set; }
    }
}
