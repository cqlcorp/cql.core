// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="OutputParam.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;

    using JetBrains.Annotations;

    /// <summary>
    /// Class OutputParam.
    /// </summary>
    public static class OutputParam
    {
        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="type">The data type.</param>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns><see cref="IDbDataParameter"/></returns>
        [NotNull]
        public static IDbDataParameter Create<T>([NotNull] string name, SqlDbType type, T? value, int? size = null)
            where T : struct
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return Create(name, type, size, value.HasValue ? (object)value.Value : DBNull.Value);
        }

        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The data type.</param>
        /// <param name="size">The size.</param>
        /// <param name="value">The value.</param>
        /// <returns><see cref="IDbDataParameter"/></returns>
        [NotNull]
        public static IDbDataParameter Create([NotNull] string name, SqlDbType type, int? size = null, [CanBeNull] object value = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            var direction = ParameterDirection.Output;

            if (value != null)
            {
                direction = ParameterDirection.InputOutput;
            }

            return new SqlParameter(name, type, size.GetValueOrDefault(GetDefaultSizeForType(type))) { Direction = direction, Value = value };
        }

        /// <summary>
        /// Gets the default size parameter for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The data type.</param>
        /// <returns><see cref="System.Int32"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="type"/> option is not supported.</exception>
        internal static int GetDefaultSizeForType(SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.BigInt:
                    return 8;
                case SqlDbType.Binary:
                    break;
                case SqlDbType.Bit:
                    return 1;
                case SqlDbType.Char:
                    break;
                case SqlDbType.DateTime:
                    return 8;
                case SqlDbType.Decimal:
                    return 9;
                case SqlDbType.Float:
                    return 8;
                case SqlDbType.Image:
                    break;
                case SqlDbType.Int:
                    return 4;
                case SqlDbType.Money:
                    return 8;
                case SqlDbType.NChar:
                    break;
                case SqlDbType.NText:
                    break;
                case SqlDbType.NVarChar:
                    break;
                case SqlDbType.Real:
                    return 4;
                case SqlDbType.UniqueIdentifier:
                    return 16;
                case SqlDbType.SmallDateTime:
                    return 4;
                case SqlDbType.SmallInt:
                    return 2;
                case SqlDbType.SmallMoney:
                    return 4;
                case SqlDbType.Text:
                    break;
                case SqlDbType.Timestamp:
                    return 8;
                case SqlDbType.TinyInt:
                    return 1;
                case SqlDbType.VarBinary:
                    break;
                case SqlDbType.VarChar:
                    break;
                case SqlDbType.Variant:
                    break;
                case SqlDbType.Xml:
                    break;
                case SqlDbType.Udt:
                    break;
                case SqlDbType.Structured:
                    break;
                case SqlDbType.Date:
                    return 3;
                case SqlDbType.Time:
                    return 3;
                case SqlDbType.DateTime2:
                    return 27;
                case SqlDbType.DateTimeOffset:
                    return 27;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }

            return -1;
        }
    }
}
