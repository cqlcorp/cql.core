using System;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.Win32.SafeHandles;

namespace Cql.NativeMethods.Logon
{
    public static class WindowsImpersonation
    {
        public static void RunImpersonated(NetworkCredential credentials, Action impersonatedAction)
        {
            var safeAccessTokenHandle = AcquireImpersonationHandle(credentials);

            WindowsIdentity.RunImpersonated(safeAccessTokenHandle, impersonatedAction);
        }

        public static T RunImpersonated<T>(NetworkCredential credentials, Func<T> impersonatedAction)
        {
            var safeAccessTokenHandle = AcquireImpersonationHandle(credentials);

            return WindowsIdentity.RunImpersonated(safeAccessTokenHandle, impersonatedAction);
        }

        public static Task RunImpersonatedAsync(NetworkCredential credentials, Task impersonatedAction)
        {
            return RunImpersonatedAsync(credentials, () => impersonatedAction);
        }

        public static async Task RunImpersonatedAsync(NetworkCredential credentials, Func<Task> impersonatedAction)
        {
            var safeAccessTokenHandle = AcquireImpersonationHandle(credentials);

            await WindowsIdentity.RunImpersonated(safeAccessTokenHandle, async () => await impersonatedAction());
        }

        public static Task<T> RunImpersonatedAsync<T>(NetworkCredential credentials, Task<T> impersonatedAction)
        {
            return RunImpersonatedAsync(credentials, () => impersonatedAction);
        }

        public static async Task<T> RunImpersonatedAsync<T>(NetworkCredential credentials, Func<Task<T>> impersonatedAction)
        {
            var safeAccessTokenHandle = AcquireImpersonationHandle(credentials);

            return await WindowsIdentity.RunImpersonated(safeAccessTokenHandle, async () => await impersonatedAction());
        }

        private static SafeAccessTokenHandle AcquireImpersonationHandle(NetworkCredential credentials)
        {
            var result = WindowsLogon.Logon(credentials);

            if (!result.Success)
            {
                result.ThrowException();
            }

            return result.Token;
        }
    }
}
