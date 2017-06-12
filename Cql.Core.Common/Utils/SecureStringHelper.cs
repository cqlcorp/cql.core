using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Cql.Core.Common.Utils
{
    public static class SecureStringHelper
    {
        public static string ConvertToUnsecureString(SecureString securePassword)
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

        public static SecureString ConvertToSecureString(string password)
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
    }
}
