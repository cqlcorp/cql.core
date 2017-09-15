namespace Cql.Core.NativeMethods.Logon
{
    using System;
    using System.Runtime.ExceptionServices;
    using System.Security.Principal;

    using Microsoft.Win32.SafeHandles;

    public class WindowsLogonResult
    {
        public ExceptionDispatchInfo Exception { get; set; }

        public string Message { get; set; }

        public bool Success { get; set; }

        public SafeAccessTokenHandle Token { get; set; }

        public static WindowsLogonResult Error(Exception ex)
        {
            return new WindowsLogonResult { Success = false, Token = SafeAccessTokenHandle.InvalidHandle, Exception = ExceptionDispatchInfo.Capture(ex), Message = ex.Message };
        }

        public static WindowsLogonResult Invalid()
        {
            return new WindowsLogonResult { Token = SafeAccessTokenHandle.InvalidHandle, Message = "Invalid login" };
        }

        public WindowsIdentity GetIdentity()
        {
            return this.Token.IsInvalid ? WindowsIdentity.GetAnonymous() : new WindowsIdentity(this.Token.DangerousGetHandle());
        }

        public void ThrowException()
        {
            this.Exception?.Throw();
        }
    }
}
