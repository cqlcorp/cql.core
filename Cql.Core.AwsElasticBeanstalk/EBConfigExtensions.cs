using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Cql.Core.AwsElasticBeanstalk
{
    public static class EBConfigExtensions
    {
        public static IConfigurationBuilder AddElasticBeanstalkConfigFile(this IConfigurationBuilder builder, IFileProvider provider = null, string path = null, bool optional = true)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = ElasticBeanstalkUtil.DefaultContainerConfigurationFilePath;
            }

            if (provider == null && Path.IsPathRooted(path))
            {
                provider = new PhysicalFileProvider(Path.GetDirectoryName(path));
                path = Path.GetFileName(path);
            }

            var source = new EBConfigurationSource
            {
                FileProvider = provider,
                Path = path,
                Optional = optional,
            };

            builder.Add(source);

            return builder;
        }
    }
}