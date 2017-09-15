namespace Cql.Core.Messaging
{
    using System.Net.Mail;

    public class MessageAddress : IMessageAddress
    {
        public string Address { get; set; }

        public string DisplayName { get; set; }

#if NET451 || CORE20
        public static implicit operator MailAddress(MessageAddress recipient)
        {
            return recipient == null ? null : new MailAddress(recipient.Address, recipient.DisplayName);
        }

#endif

        public static implicit operator MessageAddress(string address)
        {
            return new MessageAddress { Address = address };
        }

        public virtual string ToFormattedAddress()
        {
            return string.IsNullOrEmpty(this.DisplayName) ? this.Address : $"\"{this.DisplayName}\" {this.Address}";
        }

        public override string ToString()
        {
            return this.ToFormattedAddress();
        }
    }
}
