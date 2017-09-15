namespace Cql.Core.SqlServer
{
    using System;

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
            this.CurrentPage = 1;
            this.PageSize = 1;
            this.OrderByClause = $"1";
        }

        public long CurrentPage
        {
            get
            {
                if (this._currentPage < 1)
                {
                    this._currentPage = 1;
                }

                return this._currentPage;
            }

            set => this._currentPage = value;
        }

        /// <summary>
        /// The ORDER BY clause (Use $"" to make a <see cref="FormattableString" />)
        /// <para>
        /// EXAMPLE: $"{nameof(Customer.ID):C}"
        /// </para>
        /// <para>
        /// Visit https://github.com/MoonStorm/Dapper.FastCRUD/wiki/SQL-statements-and-clauses for more information.
        /// </para>
        /// </summary>
        public FormattableString OrderByClause { get; set; }

        public long PageSize
        {
            get
            {
                if (this._pageSize < 1)
                {
                    this._pageSize = 1;
                }

                return this._pageSize;
            }

            set => this._pageSize = value;
        }

        public object Parameters { get; set; }

        /// <summary>
        /// The WHERE clause (Use $"" to make a <see cref="FormattableString" />)
        /// ///
        /// <para>
        /// EXAMPLE: $"{nameof(Customer.Name):C} LIKE @CustomerName"
        /// </para>
        /// <para>
        /// Visit https://github.com/MoonStorm/Dapper.FastCRUD/wiki/SQL-statements-and-clauses for more information.
        /// </para>
        /// </summary>
        public FormattableString WhereClause { get; set; }

        public long GetSkip()
        {
            return (this.CurrentPage - 1) * this.PageSize;
        }

        public long GetTake()
        {
            return this.PageSize;
        }
    }
}
