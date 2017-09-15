namespace Cql.Core.AspNetCore.Utils
{
    internal static class BuildableUriOptionsExtensions
    {
        public static bool HasFlagFast(this BuildableUriOptions value, BuildableUriOptions flag)
        {
            return (value & flag) != 0;
        }
    }
}