// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="IDbConnectionCreator.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.SqlServer
{
    using System;
    using System.Data.Common;

    using JetBrains.Annotations;

    /// <summary>
    /// Interface IDbConnectionCreator
    /// </summary>
    public interface IDbConnectionCreator
    {
        /// <summary>
        /// Creates the database connection.
        /// </summary>
        /// <returns><see cref="DbConnection"/></returns>
        [NotNull]
        DbConnection CreateDbConnection();

        /// <summary>
        /// Raises the commands executed event.
        /// </summary>
        void RaiseCommandsExecutedEvent();

        /// <summary>
        /// Raises the execute error event.
        /// </summary>
        /// <param name="ex">The ex.</param>
        void RaiseExecuteErrorEvent([NotNull] Exception ex);
    }
}
