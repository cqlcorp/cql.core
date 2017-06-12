namespace Cql.Core.Messaging
{
    public class MessageDispatchResult
    {
        public bool Succeeded { get; set; }

        public string Errors { get; set; }

        public string MessageId { get; set; }
    }
}
