using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace Cql.Core.Messaging.Amazon
{
    internal static class RawMailHelper
    {
        private const BindingFlags NonPublicInstance = BindingFlags.Instance | BindingFlags.NonPublic;

        private static readonly ConstructorInfo MailWriterConstructor;
        private static readonly MethodInfo SendMethod;
        private static readonly MethodInfo CloseMethod;

        static RawMailHelper()
        {
            var systemAssembly = typeof(SmtpClient).Assembly;

            var mailWriterType = systemAssembly.GetType("System.Net.Mail.MailWriter");

            MailWriterConstructor = mailWriterType.GetConstructor(NonPublicInstance, null, new[] {typeof(Stream)}, null);

            SendMethod = typeof(MailMessage).GetMethod("Send", NonPublicInstance);

            CloseMethod = mailWriterType.GetMethod("Close", NonPublicInstance);
        }

        public static MemoryStream ConvertMailMessageToMemoryStream(MailMessage message)
        {
            using (var memoryStream = new MemoryStream())
            {
                var mailWriter = MailWriterConstructor.Invoke(new object[] {memoryStream});

                SendMethod.Invoke(message, NonPublicInstance, null, new[] {mailWriter, true}, null);

                CloseMethod.Invoke(mailWriter, NonPublicInstance, null, new object[] {}, null);

                return memoryStream;
            }
        }
    }
}
