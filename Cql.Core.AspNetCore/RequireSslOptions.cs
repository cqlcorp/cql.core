namespace Cql.Core.AspNetCore
{
    public class RequireSslOptions
    {
        public int SslPort { get; set; } = 443;

        public static RequireSslOptions Default()
        {
            return new RequireSslOptions
                   {
                       SslPort = 443
                   };
        }
    }
}
