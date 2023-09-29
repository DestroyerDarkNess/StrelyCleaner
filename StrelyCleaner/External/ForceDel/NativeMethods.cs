namespace MK.Tools.ForceDel
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Return codes of the NT-Functions, like NtQueryObject.
    /// </summary>
    internal enum NtStatus
    {
        /// <summary>
        /// The operation completed successfully (STATUS_SUCCESS).
        /// </summary>
        Success = 0x00000000,

        /// <summary>
        /// The data was too large to fit into the specified buffer (STATUS_BUFFER_OVERFLOW).
        /// </summary>
        BufferOverflow = unchecked((int)0x80000005L),

        /// <summary>
        /// The specified information record length does not match the length that is 
        /// required for the specified information class (STATUS_INFO_LENGTH_MISMATCH).
        /// </summary>
        InfoLengthMismatch = unchecked((int)0xC0000004L),

        /// <summary>
        /// An invalid handle was specified (STATUS_INVALID_HANDLE).
        /// </summary>
        InvalidHandle = unchecked((int)0xC0000008L)
    }

    /// <summary>
    /// This enumeration defines information classes for system settings. This type is used with 
    /// the native functions NtQuerySystemInformation and NtSetSystemInformation (SYSTEM_INFORMATION_CLASS).
    /// </summary>
    internal enum SystemInformationClass
    {
        /// <summary>
        /// Fills a buffer with a SYSTEM_PROCESS_INFORMATION structure.
        /// </summary>
        SystemProcessInformation = 5,

        /// <summary>
        /// Fills a buffer with a SYSTEM_HANDLE_ENTRY structure.
        /// </summary>
        SystemHandleInformation = 16
    }

    /// <summary>
    /// This enumeration type represents the type of information to supply about an object (OBJECT_INFORMATION_CLASS).
    /// </summary>
    internal enum ObjectInformationClass
    {
        /// <summary>
        /// A PUBLIC_OBJECT_NAME_INFORMATION structure is supplied.
        /// </summary>
        ObjectNameInformation = 1,

        /// <summary>
        /// A PUBLIC_OBJECT_TYPE_INFORMATION structure is supplied.
        /// </summary>
        ObjectTypeInformation = 2
    }

    /// <summary>
    /// This enumeration type represents required access rights passed to the native function OpenProcess (PROCESS_ACCESS_RIGHTS).
    /// </summary>
    [Flags]
    internal enum ProcessAccessRights
    {
        /// <summary>
        /// Privilege for duplication a handle (PROCESS_DUP_HANDLE).
        /// </summary>
        ProcessDuplicateHandle = 0x00000040
    }

    /// <summary>
    /// This enumeration type represents the options that can be passed to the native function DuplicateHandle.
    /// </summary>
    [Flags]
    internal enum DuplicateHandleOptions
    {
        /// <summary>
        /// Closes the source handle. This occurs regardless of any error status returned (DUPLICATE_CLOSE_SOURCE).
        /// </summary>
        CloseSource = 0x1,

        /// <summary>
        /// The duplicate handle has the same access as the source handle (DUPLICATE_SAME_ACCESS).
        /// </summary>
        SameAccess = 0x2
    }

    /// <summary>
    /// Specifies the type of application (RM_APP_TYPE).
    /// </summary>
    internal enum ApplicationType
    {
        /// <summary>
        /// The application cannot be classified as any other type. An application of this type can only be shut down by a forced shutdown.
        /// </summary>
        RmUnknownApp = 0,

        /// <summary>
        /// A Windows application run as a stand-alone process that displays a top-level window.
        /// </summary>
        RmMainWindow = 1,

        /// <summary>
        /// A Windows application that does not run as a stand-alone process and does not display a top-level window.
        /// </summary>
        RmOtherWindow = 2,

        /// <summary>
        /// The application is a Windows service.
        /// </summary>
        RmService = 3,

        /// <summary>
        /// The application is Windows Explorer.
        /// </summary>
        RmExplorer = 4,

        /// <summary>
        /// The application is a stand-alone console application.
        /// </summary>
        RmConsole = 5,

        /// <summary>
        /// A system restart is required to complete the installation because a process cannot be shut down.
        /// </summary>
        RmCritical = 1000
    }

    /// <summary>
    /// Diese Struktur liefert die Funktion NtQuerySystemInformation 
    /// für jedes Handle eines Prozesses (SYSTEM_HANDLE_ENTRY).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SystemHandleEntry
    {
        /// <summary>
        /// ID of the process that this handle belongs to.
        /// </summary>
        public int OwnerPid;

        /// <summary>
        /// Type of the object (file, directory, pipe, ..). Sadly this value may change
        /// from one Windows version to another.
        /// </summary>
        public byte ObjectType;

        /// <summary>
        /// Some special flags (but they are undocumented).
        /// </summary>
        public byte HandleFlags;

        /// <summary>
        /// Numerical value of the handle.
        /// </summary>
        public short HandleValue;

        /// <summary>
        /// Pointer to a FILE_OBJECT structure (in Kernel Space).
        /// </summary>
        public IntPtr ObjectPointer;

        /// <summary>
        /// Access mask (undocumented).
        /// </summary>
        public int AccessMask;

        /// <summary>
        /// Generates a string containing all attributes of the SystemHandleEntry.
        /// </summary>
        /// <returns>A string containg all elementy of this struct (friendly formatted).</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("OwnerPid={0}, ", this.OwnerPid);
            sb.AppendFormat("ObjectType={0:X02}, ", this.ObjectType);
            sb.AppendFormat("HandleFlags={0:X02}, ", this.HandleFlags);
            sb.AppendFormat("HandleValue={0}, ", this.HandleValue);
            sb.AppendFormat("AccessMask={0:X04}", this.AccessMask);

            return sb.ToString();
        }
    }

    /// <summary>
    /// Uniquely identifies a process by its PID and the time the process began (RM_UNIQUE_PROCESS).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct UniqueProcess
    {
        /// <summary>
        /// The process identifier (PID)..
        /// </summary>
        public int ProcessId;

        /// <summary>
        /// The creation time of the process.
        /// </summary>
        public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
    }

    /// <summary>
    /// Describes an application (RM_PROCESS_INFO).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct ProcessInfo
    {
        /// <summary>
        /// Contains an RM_UNIQUE_PROCESS structure that uniquely identifies the application by its PID and the time the process began.
        /// </summary>
        public UniqueProcess Process;

        /// <summary>
        /// If the process is a service, this parameter returns the long name for the service. If the process is not a service, 
        /// this parameter returns the user-friendly name for the application.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255 + 1)]
        public string ApplicationName;

        /// <summary>
        /// If the process is a service, this is the short name for the service. This member is not used if the process is not a service.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 63 + 1)]
        public string ServiceShortName;

        /// <summary>
        /// Contains an RM_APP_TYPE enumeration value that specifies the type of application.
        /// </summary>
        public ApplicationType ApplicationType;

        /// <summary>
        /// Contains a bit mask that describes the current status of the application.
        /// </summary>
        public uint AppStatus;

        /// <summary>
        /// Contains the Terminal Services session ID of the process. If the terminal session of the process 
        /// cannot be determined, the value of this member is set to -1.
        /// </summary>
        public uint TSSessionId;

        /// <summary>
        /// TRUE if the application can be restarted by the Restart Manager; otherwise, FALSE.
        /// This member is always TRUE if the process is a service. 
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool Restartable;
    }

    /// <summary>
    /// This class contains declarations of all required native methods.
    /// </summary>
    internal static class NativeMethods
    {
        [DllImport("ntdll.dll")]
        internal static extern NtStatus NtQuerySystemInformation(
            [In] SystemInformationClass SystemInformationClass,
            [In] IntPtr SystemInformation,
            [In] int SystemInformationLength,
            [Out] out int ReturnLength);

        [DllImport("ntdll.dll")]
        internal static extern NtStatus NtQueryObject(
            [In] IntPtr Handle,
            [In] ObjectInformationClass ObjectInformationClass,
            [In] IntPtr ObjectInformation,
            [In] int ObjectInformationLength,
            [Out] out int ReturnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern SafeNativeHandle OpenProcess(
            [In] ProcessAccessRights dwDesiredAccess,
            [In, MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
            [In] int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DuplicateHandle(
            [In] IntPtr hSourceProcessHandle,
            [In] IntPtr hSourceHandle,
            [In] IntPtr hTargetProcessHandle,
            [Out] out SafeNativeHandle lpTargetHandle,
            [In] int dwDesiredAccess,
            [In, MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
            [In] DuplicateHandleOptions dwOptions);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int GetProcessId(
            [In] IntPtr Process);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(
            [In] IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int QueryDosDevice(
            [In] string lpDeviceName,
            [Out] StringBuilder lpTargetPath,
            [In] int ucchMax);

        [DllImport("kernel32.dll")]
        internal static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentThread();

        [DllImport("kernel32.dll")]
        internal static extern bool TerminateThread(IntPtr hThread, uint dwExitCode);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        internal static extern int RmStartSession(
            out uint pSessionHandle, 
            int dwSessionFlags, 
            string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        internal static extern int RmEndSession(
            uint pSessionHandle);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        internal static extern int RmRegisterResources(
            uint pSessionHandle, 
            uint nFiles, 
            string[] rgsFilenames,
            uint nApplications, 
            UniqueProcess[] rgApplications, 
            uint nServices, 
            string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll")]
        internal static extern int RmGetList(
            uint dwSessionHandle,
            out uint pnProcInfoNeeded, 
            ref uint pnProcInfo,
            [In, Out] ProcessInfo[] rgAffectedApps,
            ref uint lpdwRebootReasons);
    }
}
