namespace Cql.Core.SqlServer
{
    using System;
    using System.Data.Common;

    public interface IDbConnectionCreator
    {
        DbConnection CreateDbConnection();

        void RaiseCommandsExecutedEvent();

        void RaiseExecuteErrorEvent(Exception ex);
    }
}
