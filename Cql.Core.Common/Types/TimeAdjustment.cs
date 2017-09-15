// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="TimeAdjustment.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Types
{
    /// <summary>
    /// Enum TimeAdjustment
    /// </summary>
    public enum TimeAdjustment
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// The start of day
        /// </summary>
        StartOfDay = 1,

        /// <summary>
        /// The end of day
        /// </summary>
        EndOfDay = 2,

        /// <summary>
        /// The start of day UTC
        /// </summary>
        StartOfDayUtc = 3,

        /// <summary>
        /// The end of day UTC
        /// </summary>
        EndOfDayUtc = 4
    }
}
