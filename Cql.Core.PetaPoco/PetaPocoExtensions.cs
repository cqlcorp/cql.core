namespace Cql.Core.PetaPoco
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using AsyncPoco;

    using Cql.Core.Common.Types;
    using Cql.Core.SqlServer;

    using SortOrder = Cql.Core.Common.Types.SortOrder;

    public static class PetaPocoExtensions
    {
        public static Task<PagedResult<T>> ExecutePagedResult<T>(this Database db, IPagingInfo paging, Sql sqlCount, Sql sqlQuery)
        {
            return db.PageAsync<T>(paging.PageNumber, paging.PageSize, sqlCount, sqlQuery).ToPagedResult();
        }

        public static Task<PagedResult<T>> ExecutePagedResult<T>(this Database db, IPagingInfo paging, Sql sql)
        {
            return db.PageAsync<T>(paging.PageNumber, paging.PageSize, sql).ToPagedResult();
        }

        public static Task<PagedResult<T>> ExecutePagedResult<T>(this Database db, IPagingInfo paging, string sql, params object[] args)
        {
            return db.PageAsync<T>(paging.PageNumber, paging.PageSize, sql, args).ToPagedResult();
        }

        public static Sql FilterByDateRange(this Sql sql, string columnName, IDateRangeFilter filter, DateRangeAdjustment rangeAdjustment = DateRangeAdjustment.None)
        {
            if (filter == null)
            {
                return sql;
            }

            if (filter.DateFrom.HasValue && filter.DateTo.HasValue)
            {
                return sql.Where($"{columnName} BETWEEN @0 AND @1", GetDateFromValue(filter.DateFrom, rangeAdjustment), GetDateToValue(filter.DateTo, rangeAdjustment));
            }

            if (filter.DateFrom.HasValue)
            {
                sql = sql.Where($"{columnName} >= @0", GetDateFromValue(filter.DateFrom, rangeAdjustment));
            }

            if (filter.DateTo.HasValue)
            {
                sql = sql.Where($"{columnName} <= @0", GetDateToValue(filter.DateTo, rangeAdjustment));
            }

            return sql;
        }

        public static Sql OrderByOptional(this Sql sql, string column, SortOrder? order)
        {
            return order.HasValue ? sql.OrderBy($"{column} {order}") : sql;
        }

        public static TQueryResult PocoQuery<TQueryResult>(this IDbConnectionCreator connectionCreator, Func<Database, TQueryResult> executeFunc)
        {
            using (var connection = connectionCreator.CreateDbConnection())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (var db = CreateDatabase(connection))
                {
                    try
                    {
                        return executeFunc(db);
                    }
                    catch (SqlException ex)
                    {
                        connectionCreator.RaiseExecuteErrorEvent(ex);
                        throw;
                    }
                    finally
                    {
                        connectionCreator.RaiseCommandsExecutedEvent();
                    }
                }
            }
        }

        public static async Task<TQueryResult> PocoQuery<TQueryResult>(this IDbConnectionCreator connectionCreator, Func<Database, Task<TQueryResult>> executeFunc)
        {
            using (var connection = connectionCreator.CreateDbConnection())
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var db = CreateDatabase(connection))
                {
                    try
                    {
                        return await executeFunc(db).ConfigureAwait(false);
                    }
                    catch (SqlException ex)
                    {
                        connectionCreator.RaiseExecuteErrorEvent(ex);
                        throw;
                    }
                    finally
                    {
                        connectionCreator.RaiseCommandsExecutedEvent();
                    }
                }
            }
        }

        /// <summary>
        /// Updates if primaryKey is greater than 0, otherwise, inserts. SaveAsync won't work in every case,
        /// but works with the 95% of cases where primary key is INT NOT NULL IDENTITY(X,X)
        /// </summary>
        /// <typeparam name="T">Type of object - must match table name.</typeparam>
        /// <param name="db">PetaPoco database</param>
        /// <param name="primaryKeyName">Name of primary key column.</param>
        /// <param name="poco">Object to save.</param>
        /// <param name="primaryKey">Value of primary key field</param>
        /// <returns></returns>
        public static Task SaveAsync<T>(this Database db, string primaryKeyName, object poco, int primaryKey)
        {
            if (primaryKey > 0)
            {
                return db.UpdateAsync(typeof(T).Name, primaryKeyName, poco);
            }

            return db.InsertAsync(typeof(T).Name, primaryKeyName, true, poco);
        }

        /// <summary>
        /// Updates if primaryKey is greater than 0, otherwise, inserts. SaveAsync won't work in every case,
        /// but works with the 95% of cases where primary key is INT NOT NULL IDENTITY(X,X)
        /// </summary>
        /// <param name="db">PetaPoco database</param>
        /// <param name="tableName">Name of database table.</param>
        /// <param name="primaryKeyName">Name of primary key column.</param>
        /// <param name="poco">Object to save.</param>
        /// <param name="primaryKey">Value of primary key field</param>
        /// <returns></returns>
        public static Task SaveAsync(this Database db, string tableName, string primaryKeyName, object poco, int primaryKey)
        {
            if (primaryKey > 0)
            {
                return db.UpdateAsync(tableName, primaryKeyName, poco);
            }

            return db.InsertAsync(tableName, primaryKeyName, true, poco);
        }

        /// <summary>
        /// Convert the PetaPoco paging object to the CPI paging object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Task<PagedResult<T>> ToPagedResult<T>(this Task<Page<T>> page)
        {
            var tcs = new TaskCompletionSource<PagedResult<T>>();
            page.ContinueWith(async t => tcs.SetResult((await t).ToPagedResult()), TaskContinuationOptions.OnlyOnRanToCompletion);
            page.ContinueWith(
                t =>
                    {
                        if (t.Exception != null)
                        {
                            tcs.SetException(t.Exception.InnerExceptions);
                        }
                    },
                TaskContinuationOptions.OnlyOnFaulted);
            page.ContinueWith(t => tcs.SetCanceled(), TaskContinuationOptions.OnlyOnCanceled);
            return tcs.Task;
        }

        /// <summary>
        /// Convert the PetaPoco paging object to the CPI paging object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        public static PagedResult<T> ToPagedResult<T>(this Page<T> page)
        {
            return new PagedResult<T>
                       {
                           CurrentPageNumber = page.CurrentPage,
                           PageSize = page.ItemsPerPage,
                           TotalRecords = page.TotalItems,
                           TotalPageCount = page.TotalPages,
                           Results = page.Items
                       };
        }

        public static Sql WhereHasValue(this Sql sql, string columnName, bool? condition)
        {
            if (!condition.HasValue)
            {
                return sql;
            }

            return sql.Where(condition.Value ? $"{columnName} IS NULL" : $"{columnName} IS NOT NULL");
        }

        /// <summary>
        /// Conditionally appends a WHERE CLAUSE statement if the <paramref name="value" /> is not <c>null</c> for each of the
        /// specified <paramref name="columns"></paramref> using an "OR" statement to join the predicate conditions.
        /// </summary>
        /// <param name="sql">The Sql Builder instance</param>
        /// <param name="columns">The column names (as they would appear in the WHERE clause)</param>
        /// <param name="value">The value</param>
        /// <param name="compare">One of the StringCompare options</param>
        /// <returns>The original Sql Builder</returns>
        public static Sql WhereOptional(this Sql sql, string[] columns, string value, StringCompare? compare)
        {
            if (columns == null || columns.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columns));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return sql;
            }

            var stringCompare = compare.GetValueOrDefault(StringCompare.Contains);

            var firstColumn = true;

            var innerSql = new Sql("WHERE (");

            foreach (var column in columns)
            {
                if (!firstColumn)
                {
                    innerSql.Append("OR ");
                }

                if (stringCompare == StringCompare.Equals)
                {
                    innerSql.Append($"({column} = @0)", value);
                }
                else
                {
                    innerSql.Append($"({column} LIKE @0)", value.WildcardSearch(stringCompare));
                }

                firstColumn = false;
            }

            innerSql.Append(")");

            return sql.Append(innerSql);
        }

        /// <summary>
        /// If value is not null, adds a where clause to the SQL.
        /// </summary>
        /// <param name="sql">The SQL</param>
        /// <param name="column">Column name to query.</param>
        /// <param name="value">Value to query.</param>
        /// <returns></returns>
        public static Sql WhereOptional<T>(this Sql sql, string column, T? value)
            where T : struct
        {
            return value.HasValue ? sql.Where($"{column} = @0", value) : sql;
        }

        /// <summary>
        /// If value is not null, adds a where clause to the SQL.
        /// </summary>
        /// <param name="sql">The SQL</param>
        /// <param name="column">Column name to query.</param>
        /// <param name="value">Value to query.</param>
        /// <param name="convert">The value formation method</param>
        /// <returns></returns>
        public static Sql WhereOptional<T, TConverted>(this Sql sql, string column, T? value, Func<T, TConverted> convert)
            where T : struct
        {
            return value.HasValue ? sql.Where($"{column} = @0", convert(value.Value)) : sql;
        }

        /// <summary>
        /// If the list isn't null and there are any non-null values in the list, add them to the SQL using an IN() query.
        /// </summary>
        /// <param name="sql">The PetaPoco SQL.</param>
        /// <param name="column">Column name to query.</param>
        /// <param name="values">Values to query.</param>
        /// <returns></returns>
        public static Sql WhereOptional(this Sql sql, string column, IEnumerable<object> values)
        {
            if (values != null && values.Any(v => v != null))
            {
                return sql.Where($"{column} IN (@0)", values);
            }

            return sql;
        }

        /// <summary>
        /// If value is not null or whitespace, adds a where clause to the SQL.
        /// </summary>
        /// <param name="sql">The SQL</param>
        /// <param name="column">Column name to query.</param>
        /// <param name="value">Value to query.</param>
        /// <param name="compare">Type of comparison - converts to LIKE with %'s in the appropriate position.</param>
        /// <returns></returns>
        public static Sql WhereOptional(this Sql sql, string column, string value, StringCompare? compare)
        {
            var stringCompare = compare.GetValueOrDefault(StringCompare.Contains);

            return string.IsNullOrWhiteSpace(value)
                       ? sql
                       : (stringCompare == StringCompare.Equals ? sql.Where($"{column} = @0", value) : sql.Where($"{column} LIKE @0", value.WildcardSearch(stringCompare)));
        }

        /// <summary>
        /// If the list isn't null and there are any non-null values in the list, add them to the SQL using an IN() query.
        /// </summary>
        /// <param name="sql">The PetaPoco SQL.</param>
        /// <param name="column">Column name to query.</param>
        /// <param name="values">Values to query.</param>
        /// <returns></returns>
        public static Sql WhereOptional(this Sql sql, string column, IEnumerable<int> values)
        {
            return values == null ? sql : sql.WhereOptional(column, values.Cast<object>());
        }

        private static Database CreateDatabase(DbConnection connection)
        {
            return new Database(connection);
        }

        private static DateTime GetDateFromValue(DateTime? dateFrom, DateRangeAdjustment rangeAdjustment)
        {
            var value = dateFrom.GetValueOrDefault();
            return rangeAdjustment == DateRangeAdjustment.Inclusive ? new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, 0, value.Kind) : value;
        }

        private static DateTime GetDateToValue(DateTime? dateTo, DateRangeAdjustment rangeAdjustment)
        {
            var value = dateTo.GetValueOrDefault();
            return rangeAdjustment == DateRangeAdjustment.Inclusive ? new DateTime(value.Year, value.Month, value.Day, 23, 59, 59, 999, value.Kind) : value;
        }
    }
}
