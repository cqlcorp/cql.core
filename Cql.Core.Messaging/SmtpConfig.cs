using System.Net;

namespace Cql.Core.Messaging
{
    public class SmtpConfig
    {
        public int Port { get; set; }

        public string Host { get; set; }

        public bool? EnableSsl { get; set; }

        public NetworkCredential Credentials { get; set; }

        public bool IgnoreCertificateErrors { get; set; }
    }
}
