using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Cql.Core.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToString<T>(this T? s, string format, IFormatProvider formatProvider = null) where T : struct, IFormattable
        {
            return s?.ToString(format, formatProvider) ?? "";
        }

        public static bool Contains(this string s, string value, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            return s.IndexOf(value, comparisonType) >= 0;
        }

        public static TValue As<TValue>(this string value)
        {
            return As(value, default(TValue));
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
             Justification = "We want to make this user friendly and return the default value on all failures")]
        public static TValue As<TValue>(this string value, TValue defaultValue)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(TValue));
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return (TValue) converter.ConvertFrom(value);
                }

                // try the other direction
                converter = TypeDescriptor.GetConverter(typeof(string));
                if (converter.CanConvertTo(typeof(TValue)))
                {
                    return (TValue) converter.ConvertTo(value, typeof(TValue));
                }
            }
            catch
            {
                // eat all exceptions and return the defaultValue, assumption is that its always a parse/format exception
            }

            return defaultValue;
        }
    }
}
