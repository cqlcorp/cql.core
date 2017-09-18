namespace Cql.Core.PetaPoco
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;

    using AsyncPoco;

    public static class StoredProcUtil
    {
        public static string FormatFunc(string functionName, params object[] parameters)
        {
            var parmList = string.Empty;

            if (parameters != null)
            {
                parmList = string.Join(",", parameters.Select((x, i) => "@" + i));
            }

            var select = string.Empty;

            if (!functionName.StartsWith("SELECT "))
            {
                select = "SELECT * FROM ";
            }

            return $"{select}{functionName}({parmList});";
        }

        public static string FormatStoredProc(string storedProcedureName, object sqlParams = null)
        {
            return $";EXEC {storedProcedureName}{FormatParameterList(sqlParams)}";
        }

        private static string FormatParameterList(object sqlParams)
        {
            return sqlParams == null ? string.Empty : $" {string.Join(", ", GetParamList(sqlParams))}";
        }

        private static string GetOutClause(object sqlParams, PropertyDescriptor prop)
        {
            var outClause = string.Empty;
            var dbParam = prop.GetValue(sqlParams) as IDbDataParameter;

            if (dbParam != null && (dbParam.Direction == ParameterDirection.Output || dbParam.Direction == ParameterDirection.InputOutput))
            {
                outClause = " OUT";
            }

            return outClause;
        }

        private static IEnumerable<string> GetParamList(object sqlParams)
        {
            return from PropertyDescriptor prop in TypeDescriptor.GetProperties(sqlParams.GetType())
                   where !PropShouldBeIgnored(sqlParams, prop)
                   let outClause = GetOutClause(sqlParams, prop)
                   select string.Format("@@{0} = @{0}{1}", prop.Name, outClause);
        }

        private static bool PropShouldBeIgnored(object sqlParams, PropertyDescriptor prop)
        {
            var dbParam = prop.GetValue(sqlParams) as IDbDataParameter;

            if (dbParam != null && dbParam.Direction == ParameterDirection.ReturnValue)
            {
                return true;
            }

            return prop.Attributes.OfType<IgnoreAttribute>().Any();
        }
    }
}
