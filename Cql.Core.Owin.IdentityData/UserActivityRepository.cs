using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Repositories;
using Cql.Core.SqlServer;

using Dapper;

namespace Cql.Core.Owin.IdentityData
{
    [SuppressMessage("ReSharper", "RedundantVerbatimPrefix")]
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public class UserActivityRepository : RepositoryBase, IUserActivityRepository
    {
        public UserActivityRepository(DatabaseConnection connection) : base(connection)
        {
        }

        public virtual Task UpdateLastActivity(int userId)
        {
            var args = new
            {
                @Id = userId
            };

            const string sql = "UPDATE dbo.AspNetUsers " +
                               "SET " +
                               " LastActivityDate = SYSDATETIMEOFFSET() " +
                               "WHERE Id = @Id";

            return Execute(db => db.ExecuteAsync(sql, args));
        }
    }
}
