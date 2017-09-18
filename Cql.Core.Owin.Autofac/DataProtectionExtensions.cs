// ***********************************************************************
// Assembly         : Cql.Core.Owin.Autofac
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="DataProtectionExtensions.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Owin.Autofac
{
    using global::Autofac;

    using global::Owin.Security.AesDataProtectorProvider;

    using JetBrains.Annotations;

    using Microsoft.Owin.Security.DataProtection;

    using AesDataProtector = global::Owin.Security.AesDataProtectorProvider.AppBuilderExtensions;

    /// <summary>
    /// Class DataProtectionExtensions.
    /// </summary>
    public static class DataProtectionExtensions
    {
        /// <summary>
        /// Registers a custom <see cref="IDataProtectionProvider" /> that does not reset the cryptography keys when the
        /// application recycles like the default Microsoft IDataProtectionProvider provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="key">A random Base64 Encoded AES-256 value that is used to encrypt Tokens in the application</param>
        /// <remarks>
        /// This is an easy way to generate a new key.
        /// http://www.digitalsanctuary.com/aes-key-generator.php
        /// </remarks>
        public static void RegisterDataProtectionProvider([NotNull] this ContainerBuilder builder, [CanBeNull] string key)
        {
            builder.Register(c => new AesDataProtectorProvider(AesDataProtector.Sha512Factory, AesDataProtector.Sha256Factory, AesDataProtector.AesFactory, key))
                .As<IDataProtectionProvider>();
        }
    }
}
