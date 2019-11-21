using Elasticsearch.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cql.Core.Elasticsearch
{
    public static class ElasticsearchExtensions
    {
        /// <summary>
        /// This will rebuild an entire index alias behind the scenes with no downtime. It creates a new index
        /// and populates it then uses Elasticsearch's hot-swapping technique to bring it online.
        /// </summary>
        public async static Task RebuildIndexWithHotSwapAsync(
            this IElasticLowLevelClient client,
            string alias,
            JObject indexMapping,
            Func<Task<IEnumerable<BulkIndexingDoc>>> reader,
            CancellationToken ctx = default(CancellationToken))
        {
            var deletableIndices = JArray.Parse((await client.CatAliasesAsync<StringResponse>(alias, new CatAliasesRequestParameters { Format = "json" }, ctx)).Body)
                .Select(x => new
                {
                    alias = x.Value<string>("alias"),
                    index = x.Value<string>("index")
                })
                .ToList();

            var index = GenerateUniqueIndexNameForAlias(alias);

            await client.IndicesCreateAsync<StringResponse>(index, PostData.String(indexMapping?.ToString()), ctx: ctx);

            while (!ctx.IsCancellationRequested)
            {
                // TODO: If an exception is thrown, delete the half-created index
                var docs = await reader();

                if (ctx.IsCancellationRequested || !docs.Any())
                {
                    break;
                }

                var body = docs.SelectMany(doc => doc.ToLines().Select(x => x.ToString(Formatting.None)));
                var bulkResponse = await client.BulkAsync<StringResponse>(index, PostData.MultiJson(body));

                ThrowOnPartialBulkSuccess(bulkResponse);
            }

            if (ctx.IsCancellationRequested)
            {
                return;
            }

            var actions = deletableIndices.Select(idx => (object)new
            {
                remove = new { idx.index, idx.alias }
            }).ToList();

            actions.Add(new { add = new { index, alias } });

            // This is the hot-swap. The actions in the list are performed atomically
            await client.IndicesUpdateAliasesForAllAsync<StringResponse>(PostData.String(JObject.FromObject(new
            {
                actions
            }).ToString()), ctx: ctx);

            if (deletableIndices.Any())
            {
                await client.IndicesDeleteAsync<StringResponse>(string.Join(",", deletableIndices.Select(x => x.index)), ctx: ctx);
            }
        }

        /// <summary>
        /// The Bulk API will return 200 even if individual items fail, so we have to check the status
        /// of each bulk item.
        /// This assumes that the elasticsearch client would have already thrown if the overall request failed.
        /// </summary>
        public static void ThrowOnPartialBulkSuccess(StringResponse bulkResponse)
        {
            if (bulkResponse != null)
            {
                var resp = JObject.Parse(bulkResponse.Body);

                if (resp.Value<bool>("errors"))
                {
                    // TODO: Improve on this error messaging instead of just dumping out json
                    throw new ApplicationException("Elasticsearch bulk operation partially failed and was aborted:\n" + resp);
                }
            }
        }

        /// <summary>
        /// This performs a bulk command against the index pointed at by an alias. This is to avoid the potential for
        /// a hotswap to change the underlying index from under us while we're reindexing. The index mapping is only
        /// used if the aliased index does not yet exist.
        /// </summary>
        public async static Task BulkTargetingAliasAsync(
            this IElasticLowLevelClient client,
            string alias,
            JObject indexMappingIfNotExists,
            Func<Task<IEnumerable<BulkIndexingDoc>>> reader,
            CancellationToken ctx = default(CancellationToken))
        {
            var targetIndices = JArray.Parse((await client.CatAliasesAsync<StringResponse>(alias, new CatAliasesRequestParameters { Format = "json" }, ctx)).Body)
                .Select(x => x.Value<string>("index"))
                .ToList();

            string index;

            if (targetIndices.Count == 0)
            {
                index = GenerateUniqueIndexNameForAlias(alias);

                await client.IndicesCreateAsync<StringResponse>(index, PostData.String(indexMappingIfNotExists?.ToString()), ctx: ctx);
                await client.IndicesUpdateAliasesForAllAsync<StringResponse>(PostData.String(JObject.FromObject(new
                {
                    actions = new[] { new { add = new { index, alias } } }
                }).ToString()), ctx: ctx);
            }
            else if (targetIndices.Count > 1)
            {
                throw new ArgumentException(
                    $"{nameof(BulkTargetingAliasAsync)} can only be used against an alias targeting a single index. The `{alias}` alias covers {targetIndices.Count} indices",
                    nameof(alias));
            }
            else
            {
                index = targetIndices.First();
            }

            while (!ctx.IsCancellationRequested)
            {
                var docs = await reader();

                if (ctx.IsCancellationRequested || !docs.Any())
                {
                    break;
                }

                var body = docs.SelectMany(doc => doc.ToLines().Select(x => x.ToString(Formatting.None)));
                var bulkResponse = await client.BulkAsync<StringResponse>(index, PostData.MultiJson(body));

                ThrowOnPartialBulkSuccess(bulkResponse);
            }
        }

        private static string GenerateUniqueIndexNameForAlias(string alias) =>
             (alias + "-" + DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmssffff")).ToLower();
    }
}
