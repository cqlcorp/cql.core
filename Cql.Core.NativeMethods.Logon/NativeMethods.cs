namespace Cql.Core.NativeMethods.Logon
{
    using System.Runtime.InteropServices;

    using Microsoft.Win32.SafeHandles;

    internal static class NativeMethods
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);
    }
}
