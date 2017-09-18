// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="PagingExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    using Cql.Core.Common.Extensions;
    using Cql.Core.Common.Types;

    using Dapper;
    using Dapper.FastCrud;

    using JetBrains.Annotations;

    /// <summary>
    /// Class PagingExtensions.
    /// </summary>
    public static class PagingExtensions
    {
        /// <summary>
        /// The order by
        /// </summary>
        private const string OrderBy = "@OrderBy";

        /// <summary>
        /// The page number
        /// </summary>
        private const string PageNumber = "@PageNumber";

        /// <summary>
        /// The page size
        /// </summary>
        private const string PageSize = "@PageSize";

        /// <summary>
        /// The sort order
        /// </summary>
        private const string SortOrder = "@Sort";

        /// <summary>
        /// Adds @PageNumber, @PageSize, @OrderBy and @Sort parameters using the supplied parameters.
        /// </summary>
        /// <typeparam name="TSortBy">The type of the t sort by.</typeparam>
        /// <param name="args">The arguments.</param>
        /// <param name="filter">The filter.</param>
        public static void AddPagingAndSortFilters<TSortBy>([NotNull] this DynamicParameters args, [NotNull] SearchFilter<TSortBy> filter)
            where TSortBy : struct
        {
            Contract.Requires(args != null);
            Contract.Requires(filter != null);

            var orderBy = filter.OrderBy.GetValueOrDefault() as Enum;

            args.Add(PageNumber, filter.PageNumber);
            args.Add(PageSize, filter.PageSize);
            args.Add(OrderBy, orderBy.GetDataValue());
            args.Add(SortOrder, filter.SortOrder.GetValueOrDefault().GetDataValue());
        }

        /// <summary>
        /// Adds @PageNumber and @PageSize parameters using the supplied filter.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="filter">The filter.</param>
        public static void AddPagingFilters([NotNull] this DynamicParameters args, [NotNull] IPagingInfo filter)
        {
            Contract.Requires(args != null);
            Contract.Requires(filter != null);

            args.Add(PageNumber, filter.PageNumber);
            args.Add(PageSize, filter.PageSize);
        }

        /// <summary>
        /// Extracts the paging info from the <see cref="DynamicParameters" /> when keys are named "@PageNumber" and "@PageSize"
        /// </summary>
        /// <param name="args">The dynamic arguments</param>
        /// <returns>And instance of <see cref="PagingInfo" />.</returns>
        [NotNull]
        public static IPagingInfo GetPagingInfo([NotNull] this DynamicParameters args)
        {
            Contract.Requires(args != null);

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
        public static int GetTotalRecords<T>([CanBeNull] this List<T> results)
        {
            if (results == null || results.Count == 0)
            {
                return 0;
            }

            var totalRecords = results.Count;

            if (results.FirstOrDefault() is ISearchResult totalCountRecord)
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

        /// <summary>
        /// Executes the count query.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="db">The database.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The count query result</returns>
        private static async Task<int> ExecuteCountQuery<T>([NotNull] IDbConnection db, [NotNull] FastCrudPagedQueryFilter filter)
        {
            Contract.Requires(db != null);
            Contract.Requires(filter != null);

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

        /// <summary>
        /// Executes the fast crud paged query.
        /// </summary>
        /// <typeparam name="T">The query result type.</typeparam>
        /// <param name="db">The database.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>An awaitable list of query results.</returns>
        private static async Task<List<T>> ExecuteFastCrudPagedQuery<T>([NotNull] IDbConnection db, [NotNull] FastCrudPagedQueryFilter filter)
        {
            Contract.Requires(db != null);
            Contract.Requires(filter != null);

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
