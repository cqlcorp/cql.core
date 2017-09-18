// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="TypeUtils.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;

    using Cql.Core.Common.Utils;

    using JetBrains.Annotations;

    /// <summary>
    /// Class TypeUtils.
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        /// The columns
        /// </summary>
        [NotNull]
        private static readonly ConcurrentDictionary<Type, string[]> Columns = new ConcurrentDictionary<Type, string[]>();

        /// <summary>
        /// The table names
        /// </summary>
        [NotNull]
        private static readonly ConcurrentDictionary<Type, string> TableNames = new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// Gets the column names.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>An array of column names.</returns>
        [NotNull]
        public static string[] GetColumnNames<T>()
        {
            return Columns.GetOrAdd(
                typeof(T),
                type => ReflectionUtils.GetTypeInfo(type).GetProperties().Select(prop => prop.GetCustomAttribute<ColumnAttribute>()?.Name ?? prop.Name).ToArray());
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>The table name from the <see cref="TableAttribute" /> or the type's name.</returns>
        [NotNull]
        public static string GetTableName<T>()
        {
            return TableNames.GetOrAdd(typeof(T), type => ReflectionUtils.GetTypeInfo(type).GetCustomAttribute<TableAttribute>()?.Name ?? type.Name);
        }
    }
}
