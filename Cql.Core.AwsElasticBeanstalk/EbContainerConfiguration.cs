using System.Threading;

using Newtonsoft.Json;

namespace Cql.Core.AwsElasticBeanstalk
{
    public class EbContainerConfiguration
    {
        private IISConfigNode _iis;

        [JsonProperty("iis")]
        public IISConfigNode IIS
        {
            get => LazyInitializer.EnsureInitialized(ref _iis);
            set => _iis = value;
        }
    }
}
