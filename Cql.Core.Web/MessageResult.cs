namespace Cql.Core.Web
{
    public class MessageResult : IMessageResult
    {
        public MessageResult()
        {
        }

        public MessageResult(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}
