namespace MK.Tools.ForceDel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Static helper class that provides methods dealing with the system
    /// (like the operating system or processes).
    /// </summary>
    public static class SystemHelper
    {
        /// <summary>
        /// Determines is the running operating system is Windows Vista or even
        /// a newer version of Microsoft Windows (like Windows 7 or Windows 8).
        /// </summary>
        /// <returns>true if and only if the running operating system is Windows Vista or newer.</returns>
        public static bool IsWindowsVistaOrNewer()
        {
            OperatingSystem os = Environment.OSVersion;
            Version windowsVersion = os.Version;

            return os.Platform == PlatformID.Win32NT && windowsVersion.Major >= 6;
        }

        /// <summary>
        /// Creates a list conatining the process IDs of all currently running processes.
        /// </summary>
        /// <returns>A list conatining the process IDs of all currently running processes.</returns>
        public static List<int> GetProcesses()
        {
            List<int> result = new List<int>();

            foreach (Process p in Process.GetProcesses())
                result.Add(p.Id);

            return result;
        }
    }
}
