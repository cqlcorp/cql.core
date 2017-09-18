namespace Cql.Core.PetaPoco
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AsyncPoco;

    public static class DatabaseExtensions
    {
        public static Task<List<TResult>> ExecuteFunc<TResult>(this Database db, string functionName, params object[] parameters)
        {
            return db.FetchAsync<TResult>(StoredProcUtil.FormatFunc(functionName, parameters), parameters);
        }

        public static Task<int> ExecuteNonQuery(this Database db, string storedProcedureName, object sqlParams = null)
        {
            return db.ExecuteAsync(StoredProcUtil.FormatStoredProc(storedProcedureName, sqlParams), sqlParams);
        }

        public static async Task<TResult> ExecuteSingleResult<TResult>(this Database db, string storedProcedureName, object sqlParams = null)
        {
            var result = await db.ExecuteStoredProc<TResult>(storedProcedureName, sqlParams);
            return result.SingleOrDefault();
        }

        public static Task<List<TResult>> ExecuteStoredProc<TResult>(this Database db, string storedProcedureName, object sqlParams = null)
        {
            return db.FetchAsync<TResult>(StoredProcUtil.FormatStoredProc(storedProcedureName, sqlParams), sqlParams);
        }
    }
}
