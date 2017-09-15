namespace Cql.Core.Messaging
{
    using System.Net;

    public class SmtpConfig
    {
        public NetworkCredential Credentials { get; set; }

        public bool? EnableSsl { get; set; }

        public string Host { get; set; }

        public bool IgnoreCertificateErrors { get; set; }

        public int Port { get; set; }
    }
}
