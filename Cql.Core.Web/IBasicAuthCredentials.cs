namespace Cql.Core.Web
{
    public interface IBasicAuthCredentials
    {
        string UserName { get; }

        string Password { get; }
    }
}
