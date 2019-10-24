using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Cql.Core.Elasticsearch
{
    /// <summary>
    /// This represents a command in the bulk indexing payload (https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-bulk.html).
    /// It will only perform `index` or `delete` commands (no partial `update` command). If the Doc is null, it will delete the ID. Otherwise
    /// it will perform an `index` command which overwrites the elasticsearch record.
    /// </summary>
    public class BulkIndexingDoc
    {
        public virtual string ID { get; set; }
        public virtual JObject Doc { get; set; }

        public IEnumerable<JObject> ToLines()
        {
            if (Doc == null)
            {
                return new[] { JObject.FromObject(new { delete = new { _id = ID } }) };
            }

            return new[] {
                JObject.FromObject(new { index = new { _id = ID } }),
                Doc
            };
        }
    }
}
