using System;

namespace Cql.Core.SqlServer
{
    public class ExecuteErrorEventArgs : EventArgs
    {
        public ExecuteErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; set; }
    }
}
