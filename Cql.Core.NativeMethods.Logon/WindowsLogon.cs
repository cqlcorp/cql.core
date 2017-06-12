using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Microsoft.Win32.SafeHandles;

namespace Cql.NativeMethods.Logon
{
    public static class WindowsLogon
    {
        public static WindowsLogonResult Logon(NetworkCredential credentials)
        {
            try
            {
                return AttemptLogon(credentials);
            }
            catch (Exception ex)
            {
                return WindowsLogonResult.Error(ex);
            }
        }

        public static Task<WindowsLogonResult> LogonAsync(NetworkCredential credentials)
        {
            return Task.Run(() => Logon(credentials));
        }

        private static WindowsLogonResult AttemptLogon(NetworkCredential credentials)
        {
            if (credentials == null)
            {
                return WindowsLogonResult.Invalid();
            }

            SafeAccessTokenHandle safeAccessTokenHandle;

            var success = NativeMethods.LogonUser(
                credentials.UserName,
                credentials.Domain,
                credentials.Password,
                Win32Logon.LOGON_INTERACTIVE,
                Win32Logon.PROVIDER_DEFAULT,
                out safeAccessTokenHandle);

            var result = new WindowsLogonResult
                         {
                             Success = success,
                             Token = safeAccessTokenHandle
                         };

            if (success)
            {
                result.Message = "Success";
            }
            else
            {
                var ex = new Win32Exception(Marshal.GetLastWin32Error());

                result.Exception = ExceptionDispatchInfo.Capture(ex);
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
