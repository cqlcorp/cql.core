using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace Cql.Core.SqlServer
{
    public static class ExtensionsForDapper
    {
        public static DbString AsVarcharDbString(this string value, string defaultValue = "")
        {
            return AsVarcharDbString(value, DbStringLength.Max, defaultValue);
        }

        public static DbString AsVarcharDbString(this string value, int maxLenth, string defaultValue = "")
        {
            return value.AsDbString(maxLenth, defaultValue, true, false);
        }

        public static DbString AsCharDbString(this string value, int maxLenth, string defaultValue = "")
        {
            return value.AsDbString(maxLenth, defaultValue, true, true);
        }

        public static DbString AsNCharDbString(this string value, int maxLenth, string defaultValue = "")
        {
            return value.AsDbString(maxLenth, defaultValue, false, true);
        }

        public static DbString AsNVarCharDbString(this string value, string defaultValue = "")
        {
            return AsNVarCharDbString(value, DbStringLength.Max, defaultValue);
        }

        public static DbString AsNVarCharDbString(this string value, int maxLenth, string defaultValue = "")
        {
            return value.AsDbString(maxLenth, defaultValue, false, false);
        }

        /// <summary>
        /// Creates a <see cref="DbString" /> Dapper parameter
        /// </summary>
        /// <param name="value">The string value</param>
        /// <param name="maxLength">The string length, use <see cref="DbStringLength.Max" /></param>
        /// for text, varchar(max) or nvarchar(max)
        /// <param name="defaultValue">The value to be used if the specified <paramref name="value" /></param>
        /// is
        /// <c>null</c>
        /// .
        /// <param name="isAnsi"><c>true</c> if the underlying string type is varchar; otherwise <c>false</c>.</param>
        /// <param name="isFixedLength"><c>true</c> if the underlying string type is char or nchar; otherwise <c>false</c>.</param>
        /// <returns>
        /// An instance of a <see cref="DbString" />.
        /// </returns>
        private static DbString AsDbString(
            this string value,
            int maxLength,
            string defaultValue = "",
            bool? isAnsi = null,
            bool? isFixedLength = null)
        {
            var s = new DbString
                    {
                        Length = maxLength,
                        Value = value.ToMaxLength(maxLength) ?? defaultValue
                    };

            if (isAnsi.HasValue)
            {
                s.IsAnsi = isAnsi.Value;
            }

            if (isFixedLength.HasValue)
            {
                s.IsFixedLength = isFixedLength.Value;
            }

            return s;
        }

        public static List<TResult> ReadToList<TSource, TResult>(this SqlMapper.GridReader reader, Func<TSource, TResult> selector)
        {
            return reader.Read<TSource>().Select(selector).ToList();
        }

        public static List<T> ReadToList<T>(this SqlMapper.GridReader reader, Func<dynamic, T> selector)
        {
            return reader.Read().Select(selector).ToList();
        }
    }
}
