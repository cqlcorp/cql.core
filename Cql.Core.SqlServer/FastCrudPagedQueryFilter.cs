// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-16-2017
// ***********************************************************************
// <copyright file="FastCrudPagedQueryFilter.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Executes a Dapper.FastCrud style paged query
    /// <para>
    /// Visit https://github.com/MoonStorm/Dapper.FastCRUD/wiki for more information.
    /// </para>
    /// </summary>
    public class FastCrudPagedQueryFilter
    {
        /// <summary>
        /// The current page
        /// </summary>
        private long currentPage;

        /// <summary>
        /// The page size
        /// </summary>
        private long pageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="FastCrudPagedQueryFilter"/> class.
        /// </summary>
        public FastCrudPagedQueryFilter()
        {
            this.CurrentPage = 1;
            this.PageSize = 1;
            this.OrderByClause = $"1";
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public long CurrentPage
        {
            get
            {
                if (this.currentPage < 1)
                {
                    this.currentPage = 1;
                }

                return this.currentPage;
            }

            set => this.currentPage = value;
        }

        /// <summary>
        /// Gets or sets the ORDER BY clause (Use $"" to make a <see cref="FormattableString" />)
        /// <para>
        /// EXAMPLE: $"{nameof(Customer.ID):C}"
        /// </para><para>
        /// Visit https://github.com/MoonStorm/Dapper.FastCRUD/wiki/SQL-statements-and-clauses for more information.
        /// </para>
        /// </summary>
        /// <value>The order by clause.</value>
        [NotNull]
        public FormattableString OrderByClause { get; set; }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public long PageSize
        {
            get
            {
                if (this.pageSize < 1)
                {
                    this.pageSize = 1;
                }

                return this.pageSize;
            }

            set => this.pageSize = value;
        }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        [CanBeNull]
        public object Parameters { get; set; }

        /// <summary>
        /// The WHERE clause (Use $"" to make a <see cref="FormattableString" />)
        /// <para>
        /// EXAMPLE: $"{nameof(Customer.Name):C} LIKE @CustomerName"
        /// </para><para>
        /// Visit https://github.com/MoonStorm/Dapper.FastCRUD/wiki/SQL-statements-and-clauses for more information.
        /// </para>
        /// </summary>
        /// <value>The where clause.</value>
        [CanBeNull]
        public FormattableString WhereClause { get; set; }

        /// <summary>
        /// Gets the number of records to skip.
        /// </summary>
        /// <returns>The number of records to skip.</returns>
        public long GetSkip()
        {
            return (this.CurrentPage - 1) * this.PageSize;
        }

        /// <summary>
        /// Gets the number of records to take.
        /// </summary>
        /// <returns>The page size.</returns>
        public long GetTake()
        {
            return this.PageSize;
        }
    }
}
