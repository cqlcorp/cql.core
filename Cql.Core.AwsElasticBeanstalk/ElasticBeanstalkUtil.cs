using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Cql.Core.Web;

using Newtonsoft.Json;

namespace Cql.Core.AwsElasticBeanstalk
{
    public static class ElasticBeanstalkUtil
    {
        public const string DefaultContainerConfigurationFilePath = @"C:\Program Files\Amazon\ElasticBeanstalk\config\containerconfiguration";

        public static OperationResult<Dictionary<string, string>> GetEnvironmentConfig(string containerConfigurationFilePath = null)
        {
            try
            {
                return ReadConfig(containerConfigurationFilePath, ReadEnvironmentConfigFile);
            }
            catch (Exception e)
            {
                return OperationResult.Error<Dictionary<string, string>>(e);
            }
        }

        public static async Task<OperationResult<Dictionary<string, string>>> GetEnvironmentConfigAsync(string containerConfigurationFilePath = null)
        {
            try
            {
                return await ReadConfig(containerConfigurationFilePath, ReadEnvironmentConfigFileAsync).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return OperationResult.Error<Dictionary<string, string>>(e);
            }
        }

        private static TResult ReadConfig<TResult>(string containerConfigurationFilePath, Func<TextReader, TResult> readerFunc)
        {
            if (string.IsNullOrEmpty(containerConfigurationFilePath))
            {
                containerConfigurationFilePath = DefaultContainerConfigurationFilePath;
            }

            using (var fs = File.Open(containerConfigurationFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    return readerFunc(sr);
                }
            }
        }

        private static OperationResult<Dictionary<string, string>> ReadEnvironmentConfig(string content)
        {
            var obj = JsonConvert.DeserializeObject<EbContainerConfiguration>(content);

            return OperationResult.Ok(obj.IIS.EnvConfig());
        }

        private static OperationResult<Dictionary<string, string>> ReadEnvironmentConfigFile(TextReader sr)
        {
            var content = sr.ReadToEnd();

            return ReadEnvironmentConfig(content);
        }

        private static async Task<OperationResult<Dictionary<string, string>>> ReadEnvironmentConfigFileAsync(TextReader sr)
        {
            var content = await sr.ReadToEndAsync();

            return ReadEnvironmentConfig(content);
        }
    }
}
