using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cql.Core.SqlServer
{
    public static class ExtensionsForXmlQueries
    {
        private static readonly ConcurrentDictionary<Type, XmlSerializer> SerializerCache = new ConcurrentDictionary<Type, XmlSerializer>();

        public static Task<TResult> ExecuteXmlResultAsync<TResult>(this IDbCommand command)
        {
            var sqlCommand = command.AsSqlCommand();

            if (sqlCommand == null)
            {
                throw new InvalidOperationException("ExecuteXmlReaderAsync method not supported.");
            }

            return sqlCommand.ExecuteXmlResultAsync<TResult>();
        }

        public static async Task<TResult> ExecuteXmlResultAsync<TResult>(this SqlCommand sqlCommand)
        {
            var type = typeof(TResult);

            var xmlSerializer = SerializerCache.GetOrAdd(type, t => new XmlSerializer(t));

            using (var r = await sqlCommand.ExecuteXmlReaderAsync())
            {
                r.MoveToContent();

                if (!r.EOF && xmlSerializer.CanDeserialize(r))
                {
                    return (TResult)xmlSerializer.Deserialize(r);
                }
            }

            return default(TResult);
        }
    }
}
