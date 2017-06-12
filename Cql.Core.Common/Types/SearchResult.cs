using Newtonsoft.Json;

namespace Cql.Core.Common.Types
{
    public class SearchResult : ISearchResult
    {
        [JsonIgnore]
        public int TotalRecords { get; set; }
    }
}
