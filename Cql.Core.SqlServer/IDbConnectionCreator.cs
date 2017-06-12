using System;
using System.Data.Common;

namespace Cql.Core.SqlServer
{
    public interface IDbConnectionCreator
    {
        DbConnection CreateDbConnection();
        void RaiseCommandsExecutedEvent();
        void RaiseExecuteErrorEvent(Exception ex);
    }
}
