namespace Cql.Core.Messaging
{
    public class MessageAddress : IMessageAddress
    {
        public string Address { get; set; }

        public string DisplayName { get; set; }

#if NET451
        public static implicit operator System.Net.Mail.MailAddress(MessageAddress recipient)
        {
            return recipient == null ? null : new System.Net.Mail.MailAddress(recipient.Address, recipient.DisplayName);
        }
#endif

        public static implicit operator MessageAddress(string address)
        {
            return new MessageAddress
            {
                Address = address
            };
        }

        public virtual string ToFormattedAddress()
        {
            return string.IsNullOrEmpty(DisplayName) ? Address : $"\"{DisplayName}\" {Address}";
        }

        public override string ToString()
        {
            return ToFormattedAddress();
        }
    }
}
