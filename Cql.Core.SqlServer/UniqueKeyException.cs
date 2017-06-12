using System.Data.Common;

namespace Cql.Core.SqlServer
{
    public class UniqueKeyException : DbException
    {
        public UniqueKeyException(string message) : base(message)
        {
        }
    }
}
