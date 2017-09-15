namespace Cql.Core.Common.Types
{
    using Newtonsoft.Json;

    public class SearchResult : ISearchResult
    {
        [JsonIgnore]
        public int TotalRecords { get; set; }
    }
}
