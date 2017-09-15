namespace Cql.Core.SqlServer
{
    using System;

    public class ExecuteErrorEventArgs : EventArgs
    {
        public ExecuteErrorEventArgs(Exception exception)
        {
            this.Exception = exception;
        }

        public Exception Exception { get; set; }
    }
}
