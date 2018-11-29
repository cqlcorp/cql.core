using Microsoft.Extensions.Configuration;

namespace Cql.Core.AwsElasticBeanstalk
{
    public class EBConfigurationSource : FileConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new EBFileConfigurationProvider(this);
        }
    }
}