using System;

namespace Cql.Core.SqlServer
{
    /// <summary>
    /// Executes a Dapper.FastCrud style paged query
    /// <para>
    /// Visit https://github.com/MoonStorm/Dapper.FastCRUD/wiki for more information.
    /// </para>
    /// </summary>
    public class FastCrudPagedQueryFilter
    {
        private long _currentPage;
        private long _pageSize;

        public FastCrudPagedQueryFilter()
        {
            CurrentPage = 1;
            PageSize = 1;
            OrderByClause = $"1";
        }

        /// <summary>
        /// The WHERE clause (Use $"" to make a <see cref="FormattableString"/>)
        /// /// <para>
        /// EXAMPLE: $"{nameof(Customer.Name):C} LIKE @CustomerName"
        /// </para>
        /// <para>
        /// Visit https://github.com/MoonStorm/Dapper.FastCRUD/wiki/SQL-statements-and-clauses for more information.
        /// </para>
        /// </summary>
        public FormattableString WhereClause { get; set; }

        /// <summary>
        /// The ORDER BY clause (Use $"" to make a <see cref="FormattableString"/>)
        /// <para>
        /// EXAMPLE: $"{nameof(Customer.ID):C}"
        /// </para>
        /// <para>
        /// Visit https://github.com/MoonStorm/Dapper.FastCRUD/wiki/SQL-statements-and-clauses for more information.
        /// </para>
        /// </summary>
        public FormattableString OrderByClause { get; set; }

        public long CurrentPage
        {
            get
            {
                if (_currentPage < 1)
                {
                    _currentPage = 1;
                }
                return _currentPage;
            }
            set { _currentPage = value; }
        }

        public long PageSize
        {
            get
            {
                if (_pageSize < 1)
                {
                    _pageSize = 1;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }

        public object Parameters { get; set; }

        public long GetSkip()
        {
            return (CurrentPage - 1) * PageSize;
        }

        public long GetTake()
        {
            return PageSize;
        }
    }
}
