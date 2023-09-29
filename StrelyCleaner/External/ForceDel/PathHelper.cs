namespace MK.Tools.ForceDel
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Static helper class containing methods for handling DOS- and device names.
    /// </summary>
    internal static class PathHelper
    {
        /// <summary>
        /// The maximum character length of a path.
        /// </summary>
        private const int MaxPathLength = 260;

        /// <summary>
        /// Prefix of network handles (handles to shares on a remote computer).
        /// </summary>
        private const string NetworkDevicePrefix = "\\Device\\LanmanRedirector\\";

        /// <summary>
        /// Helper object for synchronizing access to the deviceMap.
        /// </summary>
        private static object syncObject = new object();

        /// <summary>
        /// Dictionary with mappings of NT device names to DOS device names.
        /// </summary>
        private static Dictionary<string, string> deviceMap;

        /// <summary>
        /// This method converts a NT device name and path (like "\Device\HardDisk1\Temp\foo.txt") to a DOS path.
        /// </summary>
        /// <param name="devicePath">NT device name and path.</param>
        /// <returns>DOS path of an empty string, if the NT device name has no mapping to a DOS device name and path.</returns>
        public static string ConvertDevicePathToDosPath(string devicePath)
        {
            InitializeDeviceMapIfNeeded();
            int i = devicePath.Length;
            while (i > 0 && (i = devicePath.LastIndexOf('\\', i - 1)) != -1)
            {
                string drive = string.Empty;
                if (deviceMap.TryGetValue(devicePath.Substring(0, i), out drive))
                    return string.Concat(drive, devicePath.Substring(i));
            }

            return string.Empty;
        }

        /// <summary>
        /// Initializes the dictionary containg the mappings from NT device names to DOS device names. This initialization
        /// is done only once per application life time.
        /// </summary>
        private static void InitializeDeviceMapIfNeeded()
        {
            lock (PathHelper.syncObject)
            {
                if (deviceMap == null)
                {
                    Dictionary<string, string> localDeviceMap = BuildDeviceMap();
                    Interlocked.CompareExchange<Dictionary<string, string>>(ref deviceMap, localDeviceMap, null);
                }
            }
        }

        /// <summary>
        /// Creates the dictionary containg the mappings from NT device names to DOS device names.
        /// </summary>
        /// <returns>A dictionary containg mappings from NT device names to DOS device names.</returns>
        private static Dictionary<string, string> BuildDeviceMap()
        {
            string[] logicalDrives = Environment.GetLogicalDrives();
            Dictionary<string, string> localDeviceMap = new Dictionary<string, string>(logicalDrives.Length);
            StringBuilder targetPath = new StringBuilder(MaxPathLength);
            foreach (string drive in logicalDrives)
            {
                string deviceName = drive.Substring(0, 2);
                NativeMethods.QueryDosDevice(deviceName, targetPath, MaxPathLength);
                localDeviceMap.Add(NormalizeDeviceName(targetPath.ToString()), deviceName);
            }

            localDeviceMap.Add(NetworkDevicePrefix.Substring(0, NetworkDevicePrefix.Length - 1), "\\");
            return localDeviceMap;
        }

        /// <summary>
        /// Normalizes the NT device name. Currently this normalization just the removed the
        /// prefix "\Device\LanmanRedirector" from device names staring with this prefix.
        /// </summary>
        /// <param name="deviceName">NT device name to be normalized.</param>
        /// <returns>Normalized NT device name.</returns>
        private static string NormalizeDeviceName(string deviceName)
        {
            if (string.Compare(deviceName, 0, NetworkDevicePrefix, 0, NetworkDevicePrefix.Length, StringComparison.InvariantCulture) == 0)
            {
                string shareName = deviceName.Substring(deviceName.IndexOf('\\', NetworkDevicePrefix.Length) + 1);
                return string.Concat(NetworkDevicePrefix, shareName);
            }

            return deviceName;
        }
    }
}
