using Autofac;

using Microsoft.Owin.Security.DataProtection;

using Owin.Security.AesDataProtectorProvider;

using AesDataProtector = Owin.Security.AesDataProtectorProvider.AppBuilderExtensions;

namespace Cql.Core.Owin.Autofac
{
    public static class DataProtectionExtensions
    {
        /// <summary>
        /// Registers a custom <see cref="IDataProtectionProvider" /> that does not reset the cryptography keys when the
        /// application recycles like the default Microsoft IDataProtectionProvider provider.
        /// </summary>
        /// <remarks>
        /// This is an easy way to generate a new key.
        /// http://www.digitalsanctuary.com/aes-key-generator.php
        /// </remarks>
        /// <param name="builder"></param>
        /// <param name="key">A random Base64 Encoded AES-256 value that is used to encrypt Tokens in the application</param>
        public static void RegisterDataProtectionProvider(this ContainerBuilder builder, string key)
        {
            builder.Register(
                    c => new AesDataProtectorProvider(
                        AesDataProtector.Sha512Factory,
                        AesDataProtector.Sha256Factory,
                        AesDataProtector.AesFactory,
                        key))
                .As<IDataProtectionProvider>();
        }
    }
}
