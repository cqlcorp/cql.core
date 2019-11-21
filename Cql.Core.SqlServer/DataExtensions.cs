// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="DataExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    using Cql.Core.Common.Types;

    using Dapper;

    using JetBrains.Annotations;

    /// <summary>
    /// Extension methods for common data operations.
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// Adds a date search parameter the the parameter collection.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The database parameter type</param>
        /// <param name="timeAdjustment">The time adjustment option.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="timeAdjustment" /> option is not supported.</exception>
        public static void AddSearchParam([NotNull] this DynamicParameters args, [NotNull] string name, DateTime? value, DbType type = DbType.DateTime,
            TimeAdjustment timeAdjustment = TimeAdjustment.None)
        {
            Contract.Requires(args != null);
            Contract.Requires(!string.IsNullOrEmpty(name));

            object paramValue = DBNull.Value;

            if (value.HasValue)
            {
                var dateVal = value.Value;

                switch (timeAdjustment)
                {
                    case TimeAdjustment.None:
                        paramValue = dateVal;
                        break;
                    case TimeAdjustment.StartOfDay:
                    case TimeAdjustment.StartOfDayUtc:
                        paramValue = new DateTime(
                            dateVal.Year,
                            dateVal.Month,
                            dateVal.Day,
                            0,
                            0,
                            0,
                            0,
                            timeAdjustment == TimeAdjustment.StartOfDayUtc ? DateTimeKind.Utc : dateVal.Kind);
                        break;
                    case TimeAdjustment.EndOfDay:
                    case TimeAdjustment.EndOfDayUtc:
                        paramValue = new DateTime(
                            dateVal.Year,
                            dateVal.Month,
                            dateVal.Day,
                            23,
                            59,
                            59,
                            999,
                            timeAdjustment == TimeAdjustment.EndOfDayUtc ? DateTimeKind.Utc : dateVal.Kind);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(timeAdjustment), timeAdjustment, null);
                }
            }

            args.Add(name, paramValue, type);
        }

        /// <summary>
        /// Adds a search parameter the the parameter collection.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="args">The arguments.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The database parameter type</param>
        public static void AddSearchParam<T>([NotNull] this DynamicParameters args, [NotNull] string name, T? value, DbType type)
            where T : struct
        {
            Contract.Requires(args != null);
            Contract.Requires(!string.IsNullOrEmpty(name));

            object paramValue = DBNull.Value;

            if (value.HasValue)
            {
                paramValue = value.Value;

                if (typeof(T).GetTypeInfo().IsEnum)
                {
                    paramValue = (int) paramValue;
                }
            }

            args.Add(name, paramValue, type);
        }

        /// <summary>
        /// Adds the search parameter.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <param name="size">The size.</param>
        /// <param name="type">The database parameter type</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="searchType" /> option is not supported.</exception>
        public static void AddSearchParam(
            [NotNull] this DynamicParameters args,
            [NotNull] string name,
            [CanBeNull] string value,
            StringCompare searchType,
            int size = DbStringLength.Max,
            DbType type = DbType.String)
        {
            Contract.Requires(args != null);
            Contract.Requires(!string.IsNullOrEmpty(name));

            object paramValue = DBNull.Value;

            if (!string.IsNullOrWhiteSpace(value))
            {
                if (size != DbStringLength.Max && !value.Contains("%") && !value.Contains("_"))
                {
                    switch (searchType)
                    {
                        case StringCompare.Equals:
                            break;

                        case StringCompare.StartsWith:
                        case StringCompare.EndsWith:
                            size = size - 1;
                            break;

                        case StringCompare.Contains:
                            size = size - 2;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
                    }
                }

                paramValue = value.ToMaxLength(size).WildcardSearch(searchType);
            }

            args.Add(name, paramValue, type, ParameterDirection.Input, size);
        }

        public static bool? GetBoolean(this IDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetBoolean(i);
        }

        public static char? GetChar(this IDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetChar(i);
        }

        public static DateTime? GetDateTime(this IDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetDateTime(i);
        }

        public static DateTimeOffset? GetDateTimeOffset(this SqlDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetDateTimeOffset(i);
        }

        public static decimal? GetDecimal(this IDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetDecimal(i);
        }

        public static double? GetDouble(this IDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetDouble(i);
        }

        public static object GetFieldValue(this SqlDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.GetValue(i);
        }

        public static T GetFieldValue<T>(this SqlDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetFieldValue<T>(i);
        }

        public static float? GetFloat(this IDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetFloat(i);
        }

        public static Guid? GetGuid(this IDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetGuid(i);
        }

        public static int? GetInt16(this IDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetInt16(i);
        }

        public static int? GetInt32(this IDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetInt32(i);
        }

        public static long? GetInt64(this IDataReader reader, [NotNull] string columnName)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? default : reader.GetInt64(i);
        }

        [CanBeNull]
        public static string GetString(this IDataReader reader, [NotNull] string columnName, string defaultValue = default)
        {
            var i = reader.GetOrdinal(columnName);

            return reader.IsDBNull(i) ? defaultValue : reader.GetString(i) ?? defaultValue;
        }

        /// <summary>
        /// Gets the parameter value cast to <typeparamref name="TValue" />.
        /// </summary>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// The value cast to the specified <typeparamref name="TValue" /> or the default value of
        /// <typeparamref name="TValue" /> if the parameter value is null or <see cref="DBNull" />
        /// </returns>
        public static TValue GetValue<TValue>([NotNull] this IDbDataParameter parameter)
        {
            Contract.Requires(parameter != null);

            if (parameter.Value == null || parameter.Value is DBNull)
            {
                return default;
            }

            return (TValue) parameter.Value;
        }

        /// <summary>
        /// Truncates the specified <paramref name="value" /> if it exceeds the specified <paramref name="maxLength" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>
        ///     <see cref="System.String" />
        /// </returns>
        [CanBeNull]
        public static string ToMaxLength([CanBeNull] this string value, int? maxLength)
        {
            return maxLength.HasValue ? ToMaxLength(value, maxLength.Value) : value;
        }

        /// <summary>
        /// Truncates the specified <paramref name="value" /> if it exceeds the specified <paramref name="maxLength" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>
        ///     <see cref="System.String" />
        /// </returns>
        [CanBeNull]
        public static string ToMaxLength([CanBeNull] this string value, int maxLength)
        {
            if (maxLength == DbStringLength.Max || value == null || value.Length <= maxLength)
            {
                return value;
            }

            return value.Substring(0, maxLength);
        }

        /// <summary>
        /// Inserts wildcard search characters (%) to the string value based on the <paramref name="searchType" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <returns>
        ///     <see cref="System.String" />
        /// </returns>
        [CanBeNull]
        public static string WildcardSearch([CanBeNull] this string value, StringCompare searchType)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            value = value.Trim();

            if (value.Contains("%") || value.Contains("_"))
            {
                return value;
            }

            switch (searchType)
            {
                case StringCompare.StartsWith:
                    return $"{value}%";

                case StringCompare.EndsWith:
                    return $"%{value}";

                case StringCompare.Contains:
                    return $"%{value}%";

                default:
                    return value;
            }
        }
    }
}
