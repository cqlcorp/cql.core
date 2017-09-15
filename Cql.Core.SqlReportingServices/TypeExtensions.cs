// ReSharper disable CheckNamespace

namespace Cql.Core.ReportingServices.ReportExecution
{
    using System;
    using System.Reflection;

    public static class TypeExtensions
    {
        public static string GetEnumValue(this Enum value)
        {
            var type = value.GetType();

            var stringValue = Convert.ToString(value);

            var field = type.GetTypeInfo().GetField(stringValue);

            var attribute = field.GetCustomAttribute<EnumValueAttribute>();

            return attribute == null ? stringValue : attribute.Value;
        }
    }
}
