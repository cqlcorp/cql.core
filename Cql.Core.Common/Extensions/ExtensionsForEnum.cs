using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Cql.Core.Common.Attributes;
using Cql.Core.Common.Utils;

namespace Cql.Core.Common.Extensions
{
    public static class ExtensionsForEnum
    {
        public static string GetDescription(this Enum value)
        {
            var stringValue = Convert.ToString(value);

            var field = value.GetType().GetTypeInfo().GetField(stringValue);

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute == null ? value.GetDisplayText() : attribute.Description;
        }

        public static string GetDisplayText(this Enum value)
        {
            var stringValue = Convert.ToString(value);

            var field = value.GetType().GetTypeInfo().GetField(stringValue);

            var attribute = field.GetCustomAttribute<DisplayAttribute>();

            return attribute == null ? stringValue : attribute.GetName();
        }

        public static string GetDataValue(this Enum value)
        {
            var stringValue = Convert.ToString(value);

            var attribute = ReflectionUtils.GetFieldAttribute<DataValueAttribute>(value, stringValue);

            return attribute == null ? stringValue : attribute.Value;
        }

        public static TEnum FromDisplayText<TEnum>(this string description, TEnum? defaultValue = null) where TEnum : struct, IConvertible
        {
            return FromAttrValue<TEnum, DisplayAttribute>(description, defaultValue, attribute => attribute.GetName());
        }

        public static TEnum FromDescription<TEnum>(this string description, TEnum? defaultValue = null) where TEnum : struct, IConvertible
        {
            return FromAttrValue<TEnum, DescriptionAttribute>(description, defaultValue, attribute => attribute.Description);
        }

        public static TEnum FromDataValue<TEnum>(this string value, TEnum? defaultValue = null) where TEnum : struct, IConvertible
        {
            return FromAttrValue<TEnum, DataValueAttribute>(value, defaultValue, attribute => attribute.Value);
        }

        private static TEnum FromAttrValue<TEnum, TAttribute>(string value, TEnum? defaultValue, Func<TAttribute, string> attrValueFactory)
            where TEnum : struct
            where TAttribute : Attribute
        {
            var type = typeof(TEnum);

            var typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in typeInfo.GetFields())
            {
                var attribute = field.GetCustomAttribute<TAttribute>();

                if (attribute == null)
                {
                    if (string.Equals(field.Name, value, StringComparison.OrdinalIgnoreCase))
                    {
                        return (TEnum) field.GetValue(null);
                    }
                }
                else
                {
                    if (string.Equals(attrValueFactory(attribute), value, StringComparison.OrdinalIgnoreCase))
                    {
                        return (TEnum) field.GetValue(null);
                    }
                }
            }

            if (defaultValue.HasValue)
            {
                return defaultValue.Value;
            }

            throw new ArgumentException("Not found.", nameof(value));
        }
    }
}
