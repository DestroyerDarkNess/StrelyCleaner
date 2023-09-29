namespace MK.Tools.ForceDel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Security;

    /// <summary>
    /// This class (guess what) deletes files. It tries to delete the file using the ordinary Delete() method
    /// of the File class. If the file can not deletes this way, the class determines which process(es) has the
    /// file in use and closes the file handle of the file within the determined process(es).
    /// </summary>
    internal sealed class FileDeleter
    {
        /// <summary>
        /// All processes with all open files.
        /// </summary>
        private ProcessHandleSnapshot snapshot;

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public FileDeleter()
        {
            this.snapshot = new ProcessHandleSnapshot();
        }

        /// <summary>
        /// Deletes the file with the given name.
        /// </summary>
        /// <param name="fileName">Name of the file to be deleted.</param>
        /// <returns>true if and only the file has been successfully deleted.</returns>
        public bool Delete(string fileName)
        {
            try
            {
                string absoluteFileName = Path.GetFullPath(fileName);

                this.RemoveReadOnlyAttribute(absoluteFileName);
                this.DeleteTheFile(absoluteFileName);

                Logger.Log(LogLevel.Normal, "Deleted " + absoluteFileName);
                return true;
            }
            catch (ArgumentException ex)
            {
                Logger.Log(LogLevel.Exception, ex.Message);
            }
            catch (IOException ex)
            {
                Logger.Log(LogLevel.Exception, ex.Message);
            }
            catch (NotSupportedException ex)
            {
                Logger.Log(LogLevel.Exception, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.Log(LogLevel.Exception, ex.Message);
            }
            finally
            {
                Logger.Log(LogLevel.Verbose, string.Empty);
            }

            return false;
        }

        /// <summary>
        /// Removed the read-only attribute from the given file (if neccessary).
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>true if and only if the attribute has been removed.</returns>
        private bool RemoveReadOnlyAttribute(string fileName)
        {
            FileAttributes attr = File.GetAttributes(fileName);

            if ((attr & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
                return true;

            Logger.Log(LogLevel.Verbose, "Removing read-only attribute from file " + fileName);
            File.SetAttributes(fileName, attr & ~FileAttributes.ReadOnly);
            return true;
        }

        /// <summary>
        /// Deletes the given file.
        /// </summary>
        /// <param name="absoluteFileName">Absolute name of the file to be deleted.</param>
        /// <returns>true if and only if the file has been deleted.</returns>
        private bool DeleteTheFile(string absoluteFileName)
        {
            try
            {
                Logger.Log(LogLevel.Verbose, "Trying to delete file " + absoluteFileName);

                File.Delete(absoluteFileName);
                return true;
            }
            catch (IOException)
            {
                return this.TryHarderDelete(absoluteFileName);
            }
        }

        /// <summary>
        /// Tries to delete the file "the hard way". At first this method determines which process(es) has the
        /// file in use and closes the file handle of the file within the determined process(es). If this has
        /// been done, the file is deleted using File.Delete().
        /// </summary>
        /// <param name="absoluteFileName">Absolute name of the file to be deleted.</param>
        /// <returns>true if and only if the file has been deleted.</returns>
        private bool TryHarderDelete(string absoluteFileName)
        {
            // Windows Vista provides a new API which can be used to determine which process
            // has a file opened. This API is nor available on Windows XP so we have check
            // all processes on those legacy systems.
            bool isVistaOrNewer = SystemHelper.IsWindowsVistaOrNewer();
            List<int> processIds = isVistaOrNewer ? UsedFileDetector.GetProcesses(absoluteFileName) : SystemHelper.GetProcesses();

            foreach (int pid in processIds)
            {
                Logger.Log(LogLevel.Debug, "Checking all file handles of process " + pid);

                foreach (IntPtr handle in this.snapshot.GetHandles(pid))
                {
                    Logger.Log(LogLevel.Debug, string.Empty);

                    string fileName = LowLevelHandleHelper.GetFileNameFromHandle(handle, pid);

                    if (absoluteFileName.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.Log(LogLevel.Verbose, "File " + absoluteFileName + " is in use by process with PID " + pid);
                        this.CloseHandleInRemoteProcess(pid, handle);
                        break;
                    }
                }
            }

            File.Delete(absoluteFileName);
            return true;
        }

        /// <summary>
        /// This method closes the given handle in the given process. To archive that, the handle is duplicated using the 
        /// native method DuplicateHandle - and during the duplication the handle is closed in the remote process.
        /// </summary>
        /// <param name="processId">ID of the process hat has the specified handle opened.</param>
        /// <param name="handle">Handle to be closed.</param>
        private void CloseHandleInRemoteProcess(int processId, IntPtr handle)
        {
            SafeNativeHandle remoteProcess = NativeMethods.OpenProcess(ProcessAccessRights.ProcessDuplicateHandle, true, processId);
            if (remoteProcess.IsInvalid)
            {
                Logger.Log(LogLevel.Verbose, "Process with PID=" + processId + " could not be opened.");
                return;
            }

            IntPtr remoteProcessHandle = remoteProcess.DangerousGetHandle();
            IntPtr currentProcessHandle = NativeMethods.GetCurrentProcess();
            SafeNativeHandle duplicatedHandle = null;

            if (NativeMethods.DuplicateHandle(remoteProcessHandle, handle, currentProcessHandle, out duplicatedHandle, 0, false, DuplicateHandleOptions.CloseSource))
            {
                Logger.Log(LogLevel.Verbose, "File closed in process with PID " + processId);
                NativeMethods.CloseHandle(duplicatedHandle.DangerousGetHandle());
            }
            else
            {
                Logger.Log(LogLevel.Verbose, "File could not be closed in process with PID " + processId);
            }

            NativeMethods.CloseHandle(remoteProcessHandle);
        }
    }
}
