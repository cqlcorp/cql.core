namespace Cql.Core.Common.Utils
{
    using System;
    using System.Reflection;

    public class ReflectionUtils
    {
        public static Assembly GetAssemblyForType<T>()
        {
            return GetTypeInfo<T>().Assembly;
        }

        public static TAttribute GetCustomAttribute<TType, TAttribute>()
            where TAttribute : Attribute
        {
            return GetTypeInfo<TType>().GetCustomAttribute<TAttribute>();
        }

        public static TAttribute GetCustomAttribute<TAttribute>(object value)
            where TAttribute : Attribute
        {
            return GetTypeInfo(value).GetCustomAttribute<TAttribute>();
        }

        public static TAttribute GetCustomPropertyAttribute<TAttribute>(object value, string propertyName)
            where TAttribute : Attribute
        {
            var property = GetTypeInfo(value).GetProperty(propertyName);

            return property?.GetCustomAttribute<TAttribute>();
        }

        public static TAttribute GetFieldAttribute<TAttribute>(Enum value, string fieldName)
            where TAttribute : Attribute
        {
            var field = GetTypeInfo(value).GetField(fieldName);

            return field?.GetCustomAttribute<TAttribute>();
        }

        public static TypeInfo GetTypeInfo<T>()
        {
            return typeof(T).GetTypeInfo();
        }

        public static TypeInfo GetTypeInfo(Enum value)
        {
            return value.GetType().GetTypeInfo();
        }

        public static TypeInfo GetTypeInfo(object value)
        {
            return value.GetType().GetTypeInfo();
        }
    }
}
