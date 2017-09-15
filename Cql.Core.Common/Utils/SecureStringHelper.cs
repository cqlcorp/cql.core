// ***********************************************************************
// Assembly         : Cql.Core.Common
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="SecureStringHelper.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Cql.Core.Common.Utils
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    using JetBrains.Annotations;

    /// <summary>
    /// Class SecureStringHelper.
    /// </summary>
    public static class SecureStringHelper
    {
        /// <summary>
        /// Converts to secure string.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>A SecureString.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="password"/> cannot be null</exception>
        [NotNull]
        public static SecureString ConvertToSecureString([NotNull] string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var securePassword = new SecureString();

            foreach (var c in password)
            {
                securePassword.AppendChar(c);
            }

            securePassword.MakeReadOnly();

            return securePassword;
        }

        /// <summary>
        /// Converts to unsecure string.
        /// </summary>
        /// <param name="securePassword">The secure password.</param>
        /// <returns>A System.String.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="securePassword"/> cannot be null</exception>
        [CanBeNull]
        public static string ConvertToUnsecureString([NotNull] SecureString securePassword)
        {
            if (securePassword == null)
            {
                throw new ArgumentNullException(nameof(securePassword));
            }

            var unmanagedString = IntPtr.Zero;
            try
            {
#if COREFX
                unmanagedString = SecureStringMarshal.SecureStringToGlobalAllocUnicode(securePassword);
#else
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
#endif

                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
