using System;

namespace Cql.Core.SqlServer
{
    public class CommandsExecutedEventArgs : EventArgs
    {
        public CommandsExecutedEventArgs()
        {
        }

#if PROFILER
        public CommandsExecutedEventArgs(StackExchange.Profiling.Data.IDbProfiler profiler)
        {
            Profiler = profiler;
        }

        public StackExchange.Profiling.Data.IDbProfiler Profiler { get; set; }
#endif
    }
}
