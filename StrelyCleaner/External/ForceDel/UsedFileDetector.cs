namespace MK.Tools.ForceDel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;

    /// <summary>
    /// By querying the Windows Restart Manager, this class can determine which
    /// process has a file currently in use.
    /// </summary>
    internal static class UsedFileDetector
    {
        /// <summary>
        /// Error code "No error".
        /// </summary>
        private const int NoError = 0;

        /// <summary>
        /// Error code "Buffer too small".
        /// </summary>
        private const int ErrorMoreData = 234;

        /// <summary>
        /// Returns a list that contains all processes (pid) that have the 
        /// specified file in use.
        /// </summary>
        /// <param name="absoluteFileName">Absolute file name.</param>
        /// <returns>List with processes (pid).</returns>
        public static List<int> GetProcesses(string absoluteFileName)
        {
            // Start a Restart Manager session...
            uint sessionHandle;
            if (NativeMethods.RmStartSession(out sessionHandle, 0, Guid.NewGuid().ToString("N")) != NoError)
                throw new Win32Exception();

            List<int> processes = new List<int>();
            try
            {
                // Tell the Restart Manager, which files we are interested in...
                string[] pathStrings = new string[1];
                pathStrings[0] = absoluteFileName;

                if (NativeMethods.RmRegisterResources(sessionHandle, (uint)pathStrings.Length, pathStrings, 0, null, 0, null) != NoError)
                    throw new Win32Exception();

                // Now query the Restart Manager, which process(es) has file in use... 
                uint procInfoNeeded = 0;
                uint procInfo = 0;
                uint rebootReasons = 0;

                int result = NativeMethods.RmGetList(sessionHandle, out procInfoNeeded, ref procInfo, null, ref rebootReasons);
                while (result == ErrorMoreData)
                {
                    ProcessInfo[] processInfo = new ProcessInfo[procInfoNeeded];
                    procInfo = (uint)processInfo.Length;

                    // Note that we can get RC = ErrorMoreData again because the 
                    // process list have grown in the meantime.... 
                    result = NativeMethods.RmGetList(sessionHandle, out procInfoNeeded, ref procInfo, processInfo, ref rebootReasons);
                    if (result == NoError)
                    {
                        for (int i = 0; i < procInfo; i++)
                            processes.Add(processInfo[i].Process.ProcessId);
                    }
                }

                if (result != NoError)
                    throw new Win32Exception();
            }
            finally
            {
                NativeMethods.RmEndSession(sessionHandle);
            }

            return processes;
        }
    }
}
