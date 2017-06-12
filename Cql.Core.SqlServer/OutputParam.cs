using System;
using System.Data;
using System.Data.SqlClient;

namespace Cql.Core.SqlServer
{
    public static class OutputParam
    {
        public static IDbDataParameter Create<T>(string name, SqlDbType type, T? value, int? size = null)
            where T : struct
        {
            return Create(name, type, size, value.HasValue ? (object)value.Value : DBNull.Value);
        }

        public static IDbDataParameter Create(string name, SqlDbType type, int? size = null, object value = null)
        {
            var direction = ParameterDirection.Output;

            if (value != null)
            {
                direction = ParameterDirection.InputOutput;
            }

            return new SqlParameter(name, type, size.GetValueOrDefault(GetDefaultSizeForType(type)))
            {
                Direction = direction,
                Value = value
            };
        }

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
