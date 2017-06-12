namespace Cql.Core.Common.Types
{
    public enum TimeRangeAdjustment
    {
        /// <summary>
        /// Does not modify time
        /// </summary>
        None = 0,

        /// <summary>
        /// Adjusts the time so that DateFrom is the beginning of the day, and DateTo is the end of the day.
        /// </summary>
        Inclusive
    }
}
