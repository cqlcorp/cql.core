namespace Cql.Core.SqlServer
{
    using System.Data.Common;

    public class UniqueKeyException : DbException
    {
        public UniqueKeyException(string message)
            : base(message)
        {
        }
    }
}
