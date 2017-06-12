namespace Cql.Core.Web
{
    public class MessageResult : IMessageResult
    {
        public MessageResult()
        {
        }

        public MessageResult(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}