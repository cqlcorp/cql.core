// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="TimeRangeAdjustment.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Types
{
    /// <summary>
    /// Enum TimeRangeAdjustment
    /// </summary>
    public enum TimeRangeAdjustment
    {

        /// <summary>
        /// Does not modify time
        /// </summary>
        None = 0,

        /// <summary>
        /// Adjusts the time so that DateFrom is the beginning of the day, and DateTo is the end of the day.
        /// </summary>
        Inclusive = 1
    }
}
