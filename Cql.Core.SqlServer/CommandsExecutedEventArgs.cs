// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="CommandsExecutedEventArgs.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Diagnostics.Contracts;

    using JetBrains.Annotations;
#if PROFILER
    using StackExchange.Profiling.Data;
#endif
    /// <summary>
    /// Class CommandsExecutedEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class CommandsExecutedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsExecutedEventArgs"/> class.
        /// </summary>
        public CommandsExecutedEventArgs()
        {
        }

#if PROFILER
        public CommandsExecutedEventArgs([NotNull] IDbProfiler profiler)
        {
            Contract.Requires(profiler != null);

            this.Profiler = profiler;
        }

        [NotNull]
        public IDbProfiler Profiler { get; set; }
#endif
    }
}
