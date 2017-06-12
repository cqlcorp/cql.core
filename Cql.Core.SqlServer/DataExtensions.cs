using System;
using System.Data;
using System.Reflection;
using Cql.Core.Common.Types;
using Dapper;

namespace Cql.Core.SqlServer
{
    public static class DataExtensions
    {
        public static T GetValue<T>(this IDbDataParameter parameter)
        {
            if (parameter.Value == null || parameter.Value is DBNull)
            {
                return default(T);
            }

            return (T) parameter.Value;
        }

        public static string ToMaxLength(this string value, int? maxLength)
        {
            return maxLength.HasValue ? ToMaxLength(value, maxLength.Value) : value;
        }

        public static string ToMaxLength(this string value, int maxLength)
        {
            if (maxLength == DbStringLength.Max || value == null || value.Length <= maxLength)
            {
                return value;
            }

            return value.Substring(0, maxLength);
        }

        public static string WildcardSearch(this string value, StringCompare searchType)
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

        public static void AddSearchParam(this DynamicParameters args,
                                  string name,
                                  DateTime? value,
                                  DbType dbType,
                                  TimeAdjustment timeAdjustment = TimeAdjustment.None)
        {
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
                        paramValue = new DateTime(dateVal.Year, dateVal.Month, dateVal.Day, 0, 0, 0, 0, timeAdjustment == TimeAdjustment.StartOfDayUtc ? DateTimeKind.Utc : dateVal.Kind);
                        break;
                    case TimeAdjustment.EndOfDay:
                    case TimeAdjustment.EndOfDayUtc:
                        paramValue = new DateTime(dateVal.Year, dateVal.Month, dateVal.Day, 23, 59, 59, 999, timeAdjustment == TimeAdjustment.EndOfDayUtc ? DateTimeKind.Utc : dateVal.Kind);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(timeAdjustment), timeAdjustment, null);
                }


            }

            args.Add(name, paramValue, dbType);
        }

        public static void AddSearchParam<T>(this DynamicParameters args,
                                          string name,
                                          T? value,
                                          DbType dbType) where T : struct
        {
            object paramValue = DBNull.Value;

            if (value.HasValue)
            {
                paramValue = value.Value;

                if (typeof(T).GetTypeInfo().IsEnum)
                {
                    paramValue = (int)paramValue;
                }
            }

            args.Add(name, paramValue, dbType);
        }

        public static void AddSearchParam(this DynamicParameters args,
                                          string name,
                                          string value,
                                          StringCompare searchType,
                                          int size = DbStringLength.Max,
                                          DbType dbType = DbType.String)
        {
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

            args.Add(name, paramValue, dbType, ParameterDirection.Input, size);
        }
    }
}
