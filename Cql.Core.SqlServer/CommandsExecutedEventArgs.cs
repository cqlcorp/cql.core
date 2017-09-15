namespace Cql.Core.SqlServer
{
    using System;

    using StackExchange.Profiling.Data;

    public class CommandsExecutedEventArgs : EventArgs
    {
        public CommandsExecutedEventArgs()
        {
        }

#if PROFILER
        public CommandsExecutedEventArgs(IDbProfiler profiler)
        {
            this.Profiler = profiler;
        }

        public IDbProfiler Profiler { get; set; }
#endif
    }
}
