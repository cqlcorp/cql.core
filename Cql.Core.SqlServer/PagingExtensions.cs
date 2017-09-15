namespace Cql.Core.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    using Cql.Core.Common.Extensions;
    using Cql.Core.Common.Types;

    using Dapper;
    using Dapper.FastCrud;

    public static class PagingExtensions
    {
        private const string OrderBy = "@OrderBy";

        private const string PageNumber = "@PageNumber";

        private const string PageSize = "@PageSize";

        private const string SortOrder = "@Sort";

        /// <summary>
        /// Adds @PageNumber, @PageSize, @OrderBy and @Sort parameters using the supplied parameters.
        /// </summary>
        public static void AddPagingAndSortFilters<TSortBy>(this DynamicParameters args, SearchFilter<TSortBy> filter)
            where TSortBy : struct
        {
            var orderBy = filter.OrderBy.GetValueOrDefault() as Enum;

            args.Add(PageNumber, filter.PageNumber);
            args.Add(PageSize, filter.PageSize);
            args.Add(OrderBy, orderBy.GetDataValue());
            args.Add(SortOrder, filter.SortOrder.GetValueOrDefault().GetDataValue());
        }

        /// <summary>
        /// Adds @PageNumber and @PageSize parameters using the supplied filter.
        /// </summary>
        public static void AddPagingFilters(this DynamicParameters args, IPagingInfo filter)
        {
            args.Add(PageNumber, filter.PageNumber);
            args.Add(PageSize, filter.PageSize);
        }

        /// <summary>
        /// Extracts the paging info from the <see cref="DynamicParameters" /> when keys are named "@PageNumber" and "@PageSize"
        /// </summary>
        /// <param name="args">The dynamic arguments</param>
        /// <returns>
        /// And instance of <see cref="PagingInfo" />.
        /// </returns>
        public static IPagingInfo GetPagingInfo(this DynamicParameters args)
        {
            var pageNumber = args.Get<long>(PageNumber);
            var pageSize = args.Get<long>(PageSize);

            return new PagingInfo(pageNumber, pageSize);
        }

        /// <summary>
        /// Gets the total record count from a dataset where <typeparamref name="T" /> is an instance of
        /// <see cref="ISearchResult" />.
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="results">The list of results</param>
        /// <returns>A number indicating the total number of records available.</returns>
        public static int GetTotalRecords<T>(this List<T> results)
        {
            if (results == null || results.Count == 0)
            {
                return 0;
            }

            var totalRecords = results.Count;

            var totalCountRecord = results.FirstOrDefault() as ISearchResult;

            if (totalCountRecord != null)
            {
                totalRecords = totalCountRecord.TotalRecords;
            }

            return totalRecords;
        }

        /// <summary>
        /// Executes a Dapper.FastCrud style paged query
        /// <para>
        /// Visit https://github.com/MoonStorm/Dapper.FastCRUD/wiki/SQL-statements-and-clauses for more information.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="db">The database connection</param>
        /// <param name="filter">The filter specifying the criteria for the query.</param>
        /// <returns>A <see cref="PagedResult{T}" /> with the paging result information and dataset.</returns>
        public static async Task<PagedResult<T>> PagedQueryAsync<T>(this DbConnection db, FastCrudPagedQueryFilter filter)
        {
            return new PagedResult<T>(new PagingInfo(filter.CurrentPage, filter.PageSize))
                       {
                           TotalRecords = await ExecuteCountQuery<T>(db, filter),
                           Results = await ExecuteFastCrudPagedQuery<T>(db, filter)
                       };
        }

        private static async Task<int> ExecuteCountQuery<T>(IDbConnection db, FastCrudPagedQueryFilter filter)
        {
            return await db.CountAsync<T>(
                       s =>
                           {
                               if (filter.WhereClause != null)
                               {
                                   s.Where(filter.WhereClause);
                               }

                               s.WithParameters(filter.Parameters);
                           });
        }

        private static async Task<List<T>> ExecuteFastCrudPagedQuery<T>(IDbConnection db, FastCrudPagedQueryFilter filter)
        {
            var records = await db.FindAsync<T>(
                              s =>
                                  {
                                      if (filter.WhereClause != null)
                                      {
                                          s.Where(filter.WhereClause);
                                      }

                                      s.OrderBy(filter.OrderByClause).Skip(filter.GetSkip()).Top(filter.GetTake()).WithParameters(filter.Parameters);
                                  });

            return records.ToList();
        }
    }
}
