namespace Cql.Core.SqlServer
{
    using System;
    using System.Data.SqlClient;

    using Cql.Core.Common.Extensions;

    public static class ExtensionsForSqlExceptions
    {
        public static bool IsUniqueKeyConstaintViolation(this SqlException ex)
        {
            return ex.Message.Contains("Violation of UNIQUE KEY constraint", StringComparison.OrdinalIgnoreCase);
        }
    }
}
