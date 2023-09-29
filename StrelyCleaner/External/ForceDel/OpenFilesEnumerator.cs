namespace MK.Tools.ForceDel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Helper class for enumerating all handles of all processes.
    /// </summary>
    internal sealed class OpenFilesEnumerator : IEnumerable<SystemHandleEntry>
    {
        /// <summary>
        /// Factory method that returns the enumerator.
        /// </summary>
        /// <returns>An enumerator over all handles.</returns>
        public IEnumerator<SystemHandleEntry> GetEnumerator()
        {
            NtStatus ret;
            int length = 0x10000;

            do
            {
                IntPtr ptr = IntPtr.Zero;

                RuntimeHelpers.PrepareConstrainedRegions();
                try
                {
                    // Use a CER (Constrained Execution Region) so our AllocHGlobal()
                    // even gets executed if a exception is thrown (async) somewhere...
                    RuntimeHelpers.PrepareConstrainedRegions();
                    try
                    {
                    }
                    finally
                    {
                        ptr = Marshal.AllocHGlobal(length);
                    }

                    int returnLength;
                    Logger.Log(LogLevel.Debug, "Calling NtQueryObject(SystemHandleInformation)) width a buffersize of " + length);
                    ret = NativeMethods.NtQuerySystemInformation(SystemInformationClass.SystemHandleInformation, ptr, length, out returnLength);

                    // We need more memory? ok, let's go up to the next 64 KByte boundary...
                    if (ret == NtStatus.InfoLengthMismatch)
                    {
                        length = (returnLength + 0xffff) & ~0xffff;
                        continue;
                    }

                    if (ret == NtStatus.Success)
                    {
                        int handleCount = IntPtr.Size == 4 ? Marshal.ReadInt32(ptr) : (int)Marshal.ReadInt64(ptr);
                        int offset = IntPtr.Size;
                        int size = Marshal.SizeOf(typeof(SystemHandleEntry));

                        Logger.Log(LogLevel.Debug, "Enumerating " + handleCount + " handles");
                        for (int i = 0; i < handleCount; i++)
                        {
                            SystemHandleEntry handleEntry = (SystemHandleEntry)Marshal.PtrToStructure((IntPtr)((int)ptr + offset), typeof(SystemHandleEntry));
                            Logger.Log(LogLevel.Debug, "Handle #" + i + ": " + handleEntry.ToString());
                            yield return handleEntry;

                            offset += size;
                        }
                     }
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
            while (ret == NtStatus.InfoLengthMismatch);
        }

        /// <summary>
        /// This method returns the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
