using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Newtonsoft.Json;

namespace Cql.Core.AwsElasticBeanstalk
{
    public class IISConfigNode
    {
        private string[] _env;

        [JsonProperty("env")]
        public string[] Env
        {
            get => LazyInitializer.EnsureInitialized(ref _env, () => new string[] {});
            set => _env = value;
        }

        public Dictionary<string, string> EnvConfig()
        {
            return Env.Select(x => x.Split(new [] {'='}, StringSplitOptions.RemoveEmptyEntries)).ToDictionary(k => k[0], v => v[1], StringComparer.OrdinalIgnoreCase);
        }
    }
}
