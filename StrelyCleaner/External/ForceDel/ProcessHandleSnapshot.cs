namespace MK.Tools.ForceDel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <summary>
    /// Static helper class that creates a snapshot of all running processes and their currently opened handles.
    /// </summary>
    internal class ProcessHandleSnapshot
    {
        /// <summary>
        /// Dictionary holding all processes (key) and a list of handles (value) that the process has currently opened.
        /// </summary>
        private Dictionary<int, List<IntPtr>> snapshot;

        /// <summary>
        /// Default constructor. When this constructor is invoked a snapshot of all running processes and
        /// their currently open handles is taken. 
        /// </summary>
        public ProcessHandleSnapshot()
        {
            this.snapshot = new Dictionary<int, List<IntPtr>>();

            OpenFilesEnumerator openFiles = new OpenFilesEnumerator();
            IEnumerator<SystemHandleEntry> enumerator = openFiles.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SystemHandleEntry entry = enumerator.Current;
                int processId = entry.OwnerPid;
                IntPtr handle = (IntPtr)entry.HandleValue;

                if (LowLevelHandleHelper.IsFileHandle(handle, processId))
                {
                    if (!this.IgnoreSystemHandleEntry(entry))
                    {
                        if (!this.snapshot.ContainsKey(processId))
                            this.snapshot.Add(processId, new List<IntPtr>());

                        List<IntPtr> handleList;
                        if (this.snapshot.TryGetValue(processId, out handleList))
                            handleList.Add(handle);
                    }
                }
            }

            Logger.Log(LogLevel.Debug, "Process handle snapshot contains " + this.snapshot.Count + " entries");
        }

        /// <summary>
        /// This methods returns a collection of all handles that the process had opened when the snapshot has been taken.
        /// </summary>
        /// <param name="p">Process which handles shall be returned.</param>
        /// <returns>Collection of all handles that the process had opened when the snapshot has been taken.</returns>
        public ReadOnlyCollection<IntPtr> GetHandles(Process p)
        {
            return this.GetHandles(p.Id);
        }

        /// <summary>
        /// This methods returns a collection of all handles that the process had opened when the snapshot has been taken.
        /// </summary>
        /// <param name="processId">ID of the process which handles shall be returned.</param>
        /// <returns>Collection of all handles that the process had opened when the snapshot has been taken.</returns>
        public ReadOnlyCollection<IntPtr> GetHandles(int processId)
        {
            List<IntPtr> handleList;
            if (this.snapshot.TryGetValue(processId, out handleList))
                return new ReadOnlyCollection<IntPtr>(handleList);
            else
                return new ReadOnlyCollection<IntPtr>(new List<IntPtr>(0));
        }

        /// <summary>
        /// On Windows XP, NtQueryObject will block forever if it is called with some special handles. This method checks
        /// if the given SystemHandleEntry should be ignored.
        /// </summary>
        /// <param name="entry">SystemHandleEntry to be checked.</param>
        /// <returns>This methods returns true if and only if the specified SystemHandleEntry should be ignored.</returns>
        private bool IgnoreSystemHandleEntry(SystemHandleEntry entry)
        {
            // Calling NtQueryObject on Vista or newer may block on some handles - but we call NtQueryObject
            // in a thread and we can terminate the thread successfully (on Windows XP the thread will block
            // forever even if it is terminated)....
            if (SystemHelper.IsWindowsVistaOrNewer())
                return false;

            // Calling NtQueryObject with handles with these access masks will cause NtQueryObject
            // to block forever (it's a kernel bug on Windows XP). So we will ignore those handles...
            return entry.AccessMask == 0x0012019F || entry.AccessMask == 0x001A019F || entry.AccessMask == 0x00120189 || entry.AccessMask == 0x00100000;
        }
    }
}
