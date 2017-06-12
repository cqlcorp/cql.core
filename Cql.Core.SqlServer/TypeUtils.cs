using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Cql.Core.Common.Utils;

namespace Cql.Core.SqlServer
{
    public static class TypeUtils
    {
        private static readonly ConcurrentDictionary<Type, string[]> _columns = new ConcurrentDictionary<Type, string[]>();

        private static readonly ConcurrentDictionary<Type, string> _tableNames = new ConcurrentDictionary<Type, string>();

        public static string[] GetColumnNames<T>()
        {
            return _columns.GetOrAdd(
                typeof(T),
                type =>
                    ReflectionUtils.GetTypeInfo(type)
                                   .GetProperties()
                                   .Select(prop => prop.GetCustomAttribute<ColumnAttribute>()?.Name ?? prop.Name)
                                   .ToArray());
        }

        public static string GetTableName<T>()
        {
            return _tableNames.GetOrAdd(
                typeof(T),
                type => ReflectionUtils.GetTypeInfo(type).GetCustomAttribute<TableAttribute>()?.Name ?? type.Name);
        }
    }
}
