using System.IO;

using Microsoft.Extensions.Configuration;

namespace Cql.Core.AwsElasticBeanstalk
{
    public class EBFileConfigurationProvider : FileConfigurationProvider
    {
        public EBFileConfigurationProvider(FileConfigurationSource source) : base(source)
        {
        }

        public override void Load(Stream stream)
        {
            Data = ElasticBeanstalkUtil.ReadEnvironmentConfigFile(stream);
        }
    }
}

