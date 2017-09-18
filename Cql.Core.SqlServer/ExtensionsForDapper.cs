// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="ExtensionsForDapper.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Dapper;

    using JetBrains.Annotations;

    /// <summary>
    /// Class ExtensionsForDapper.
    /// </summary>
    public static class ExtensionsForDapper
    {
        /// <summary>
        /// Creates a <see cref="DbString" /> for the char data type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="DbString"/></returns>
        public static DbString AsCharDbString(this string value, int maxLength, string defaultValue = "")
        {
            return value.AsDbString(maxLength, defaultValue, true, true);
        }

        /// <summary>
        /// Creates a <see cref="DbString" /> for the nchar data type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="DbString"/></returns>
        public static DbString AsNCharDbString(this string value, int maxLength, string defaultValue = "")
        {
            return value.AsDbString(maxLength, defaultValue, false, true);
        }

        /// <summary>
        /// Creates a <see cref="DbString" /> for the nvarchar data type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="DbString"/></returns>
        public static DbString AsNVarCharDbString(this string value, string defaultValue = "")
        {
            return AsNVarCharDbString(value, DbStringLength.Max, defaultValue);
        }

        /// <summary>
        /// Creates a <see cref="DbString" /> for the varchar data type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="DbString"/></returns>
        public static DbString AsNVarCharDbString(this string value, int maxLength, string defaultValue = "")
        {
            return value.AsDbString(maxLength, defaultValue, false, false);
        }

        /// <summary>
        /// Creates a <see cref="DbString" /> for the varchar data type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="DbString"/></returns>
        public static DbString AsVarcharDbString(this string value, string defaultValue = "")
        {
            return AsVarcharDbString(value, DbStringLength.Max, defaultValue);
        }

        /// <summary>
        /// Creates a <see cref="DbString" /> for the varchar data type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="DbString"/></returns>
        public static DbString AsVarcharDbString(this string value, int maxLength, string defaultValue = "")
        {
            return value.AsDbString(maxLength, defaultValue, true, false);
        }

        /// <summary>
        /// Reads to list.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="selector">The selector.</param>
        /// <returns><see cref="List{TResult}"/></returns>
        [NotNull]
        public static List<TResult> ReadToList<TSource, TResult>([NotNull] this SqlMapper.GridReader reader, [NotNull] Func<TSource, TResult> selector)
        {
            Contract.Requires(reader != null);
            Contract.Requires(selector != null);

            return reader.Read<TSource>().Select(selector).ToList();
        }

        /// <summary>
        /// Reads to list.
        /// </summary>
        /// <typeparam name="TResult">THe result type.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="selector">The selector.</param>
        /// <returns><see cref="List{TResult}"/></returns>
        [NotNull]
        public static List<TResult> ReadToList<TResult>([NotNull] this SqlMapper.GridReader reader, [NotNull] Func<dynamic, TResult> selector)
        {
            Contract.Requires(reader != null);
            Contract.Requires(selector != null);

            return reader.Read().Select(selector).ToList();
        }

        /// <summary>
        /// Creates a <see cref="DbString" /> Dapper parameter
        /// </summary>
        /// <param name="value">The string value</param>
        /// <param name="maxLength">The string length, use <see cref="DbStringLength.Max" /></param>
        /// <param name="defaultValue">The value to be used if the specified <paramref name="value" /></param>
        /// <param name="isAnsi"><c>true</c> if the underlying string type is varchar; otherwise <c>false</c>.</param>
        /// <param name="isFixedLength"><c>true</c> if the underlying string type is char or nchar; otherwise <c>false</c>.</param>
        /// <returns>An instance of a <see cref="DbString" /> for text, varchar(max) or nvarchar(max) is <c>null</c>.
        /// </returns>
        [NotNull]
        private static DbString AsDbString([CanBeNull] this string value, int maxLength, [CanBeNull] string defaultValue = "", bool? isAnsi = null, bool? isFixedLength = null)
        {
            var s = new DbString { Length = maxLength, Value = value.ToMaxLength(maxLength) ?? defaultValue };

            if (isAnsi.HasValue)
            {
                s.IsAnsi = isAnsi.Value;
            }

            if (isFixedLength.HasValue)
            {
                s.IsFixedLength = isFixedLength.Value;
            }

            return s;
        }
    }
}
