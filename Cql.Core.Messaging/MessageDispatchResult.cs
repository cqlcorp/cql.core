namespace Cql.Core.Messaging
{
    public class MessageDispatchResult
    {
        public string Errors { get; set; }

        public string MessageId { get; set; }

        public bool Succeeded { get; set; }
    }
}
