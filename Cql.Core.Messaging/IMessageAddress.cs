namespace Cql.Core.Messaging
{
    public interface IMessageAddress
    {
        string Address { get; set; }

        string DisplayName { get; set; }
    }
}
