namespace Cql.Core.Web
{
    public interface IBasicAuthCredentials
    {
        string Password { get; }

        string UserName { get; }
    }
}
