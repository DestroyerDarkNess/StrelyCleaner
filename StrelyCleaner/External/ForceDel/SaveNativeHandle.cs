namespace MK.Tools.ForceDel
{
    using System;
    using System.Security.Permissions;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// This class encapsulates a native handle (File Handle, Process Handle, ...).
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    internal sealed class SafeNativeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Minimalistic (standard) constructor.
        /// </summary>
        private SafeNativeHandle()
            : base(true)
        {
        }

        /// <summary>
        /// Closes the native handle using the Windows API function CloseHandle.
        /// </summary>
        /// <returns>true if and only when the handle has been closed.</returns>
        protected override bool ReleaseHandle()
        {
            bool Result = false;
            try { Result = NativeMethods.CloseHandle(this.handle); } catch { }
            return Result;
        }
    }
}
