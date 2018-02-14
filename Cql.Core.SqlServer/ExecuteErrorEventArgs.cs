// ***********************************************************************
// Assembly         : Cql.Core.SqlServer
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="ExecuteErrorEventArgs.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.SqlServer
{
    using System;
    using System.Diagnostics.Contracts;

    using JetBrains.Annotations;

    using StackExchange.Profiling.Data;

    /// <summary>
    /// Event arguments for a <see cref="RepositoryBase" /> error event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ExecuteErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteErrorEventArgs"/> class.
        /// </summary>
        /// <param name="profiler">
        /// The database profiler.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="exception"/> cannot be null.
        /// </exception>
        public ExecuteErrorEventArgs([NotNull] IDbProfiler profiler, [NotNull] Exception exception)
        {
            Contract.Requires(profiler != null);
            Contract.Requires(exception != null);

            this.Profiler = profiler;
            this.Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        [NotNull]
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the profiler.
        /// </summary>
        [NotNull]
        public IDbProfiler Profiler { get; set; }
    }
}
