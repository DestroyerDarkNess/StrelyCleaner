using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Management;
using Microsoft.Win32;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.IO;

namespace StrelyCleaner.Core.HardwareInfo
{
   

    
        /// <summary>
        /// Information about installed software
        /// </summary>
        public class SoftwareInfo : IEquatable<SoftwareInfo>, IComparable<SoftwareInfo>
        {
            public string Name = "";
            public string Company = "";
            public string Version = "";
            public string ID = "";
            public bool Is64bit = false;

            public bool Equals(SoftwareInfo info)
            {
                if (info.Name == this.Name) return true;
                else return false;
            }

            public int CompareTo(SoftwareInfo other)
            {
                return Name.CompareTo(other.Name);
            }
        }

        /// <summary>
        /// Information about connected monitors (can be multiple)
        /// </summary>
        public class MonitorInfo
        {
            public string SerialNumber = "";
            public string Name = "";
            public string ManufactureYear = ""; // not used right now
            public string ManufactureWeek = ""; // not used right now
            public string PNPDeviceID = "";
            public bool valid = false; // indicates if monitor is actually connected or not
        }

        /// <summary>
        /// Provides basic information about attached drives in GB
        /// </summary>
        public class DriveInfo
        {
            public char DriveLetter = ' ';
            public int TotalSize = 0;
            public int FreeSpace = 0;
        }

        /// <summary>
        /// ComputerInfo class - Provides properties of various WMI data
        /// </summary>
        public class Info
        {
            /// <summary>
            /// Returns string found in EDID byte array
            /// </summary>
            /// <param name="edid">EDID byte array</param>
            /// <param name="startpos">Starting position of string to get</param>
            /// <returns>Found string</returns>
            static private string StringInEDID(Byte[] edid, int startpos)
            {
                string str = "";
                int pos = startpos;

                while (pos < edid.Length)
                {
                    // Stop if we encounter 0x00 or 0xa (line feed)
                    if (edid[pos] == 0x0 || edid[pos] == 0xa) return str.Trim();
                    else
                    {
                        char ch = (char)edid[pos];
                        str = str + ch;
                        pos++;
                    }
                }

                // We got to end.  Return string.  This probably shouldn't happen
                return str;
            }

            /// <summary>
            /// Finds a byte array inside of a byte array.  Returns position AFTER found array
            /// </summary>
            /// <param name="inArray">byte array to search</param>
            /// <param name="findArray">byte array to search for</param>
            /// <returns>Position in array AFTER found array</returns>
            static private int Find(Byte[] inArray, Byte[] findArray)
            {
                // Max position to search
                int Max = inArray.Length - findArray.Length;

                // Search for array within array
                int outpos, inpos;
                for (outpos = 0; outpos < Max; outpos++)
                {
                    for (inpos = 0; inpos < findArray.Length; inpos++)
                    {
                        // Break out of loop if we find a mismatch
                        if (inArray[outpos + inpos] != findArray[inpos]) break;
                    }

                    // Check if we got to end of findArray
                    if (inpos == findArray.Length) return outpos + findArray.Length;
                }

                // If we get here, we didn't find it.  Return -1.
                return -1;
            }


            /// <summary>
            /// Gets property value from WMIClass instance.
            /// </summary>
            static private string GetProp(string WMIClass, string WMIProperty)
            {
                // Get WMI Class
                ManagementClass theClass = new ManagementClass(WMIClass);

                // Retreive the system instances
                ManagementObjectCollection theInstances = theClass.GetInstances();

                // Loop through instances and grab data from first instance
                // (most classes only have one instance)
                foreach (ManagementObject theInstance in theInstances)
                {
                    try
                    {
                        return theInstance.Properties[WMIProperty].Value.ToString().Trim();
                    }
                    catch (NullReferenceException) // If fails for some reason, return empty string
                    {
                        return "";
                    }
                }

                // If we get here then there were NO instances.  Return empty string
                return "";
            }

            /// <summary>
            /// Return an array of property values.  Used for WMI properties that return more than 1 item.
            /// </summary>
            static private ArrayList GetProps(string WMIClass, string WMIProperty)
            {
                ArrayList items = new ArrayList(10);

                // Get WMI Class
                ManagementClass theClass = new ManagementClass(WMIClass);

                // Retreive the system instances
                ManagementObjectCollection theInstances = theClass.GetInstances();

                // Loop through instances and grab data from first instance
                // (most classes only have one instance)
                foreach (ManagementObject theInstance in theInstances)
                {
                    try
                    {
                        items.Add(theInstance.Properties[WMIProperty].Value.ToString().Trim());
                    }
                    catch (NullReferenceException) // If fails, don't add an item.  Just ignore
                    {
                    }
                }

                // If we get here then there were NO instances.  Return null
                return items;
            }

            /// <summary>
            /// Serial number of computer
            /// </summary>
            static public string GetComputerSerialNumber()
            {
                string serialNumber = GetProp("Win32_BIOS", "SerialNumber");
                if (serialNumber.Length < 3) // Older PC's may NOT have serial number!!  LAME!!!!
                {
                    serialNumber = GetProp("Win32_SystemEnclosure", "SerialNumber");
                    // Check one last time to see if we got it.
                    if (serialNumber.Length < 3)
                    {
                        // If we failed again, set serial number to PC name.
                        serialNumber = GetComputerName();
                    }
                }
                return serialNumber;
            }

            /// <summary>
            /// Manufacturer of computer
            /// </summary>
            static public string GetComputerManufacturer()
            {
                return GetProp("Win32_ComputerSystem", "Manufacturer");
            }

            /// <summary>
            /// Model of computer
            /// </summary>
            static public string GetComputerModel()
            {
                return GetProp("Win32_ComputerSystem", "Model");
            }

            /// <summary>
            /// Name of computer (usually DNS name)
            /// </summary>
            static public string GetComputerName()
            {
                return GetProp("Win32_ComputerSystem", "Name");
            }

            /// <summary>
            /// Amount of PC memory in MiB
            /// </summary>
            static public string GetMemory()
            {
                UInt64 intMemory = 0;
                string mem = GetProp("Win32_ComputerSystem", "TotalPhysicalMemory");
                try
                {
                    intMemory = UInt64.Parse(mem);
                    intMemory = intMemory / 1024 / 1024; // Convert to MiB
                }
                catch (Exception) // Catch ALL exceptions
                {
                    return "0"; // Return 0 if there's a problem
                }

                return intMemory.ToString();
            }

            /// <summary>
            /// Clock speed of computer in MHZ
            /// </summary>
            static public string GetClockSpeed()
            {
                return GetProp("Win32_Processor", "MaxClockSpeed");
            }

            /// <summary>
            /// Gets the CPU L2 Cache size in kb
            /// </summary>
            /// <returns></returns>
            static public int GetCacheSize()
            {
                int intMem = 0;
                string mem = GetProp("Win32_Processor", "L2CacheSize"); // size in kb
                try
                {
                    intMem = int.Parse(mem);
                }
                catch (Exception)
                {
                    return 0;
                }
                return intMem;
            }

            /// <summary>
            /// Returns the number of CPU Cores (usually 1-8)
            /// May not work for older OS's or CPUs
            /// </summary>
            /// <returns></returns>
            static public int GetCPUCores()
            {
                int intCores;

                try // May fail if the WMI property doesn't exist
                {
                    string cores = GetProp("Win32_Processor", "NumberOfCores");
                    intCores = int.Parse(cores);
                }
                catch (Exception)
                {
                    return 1; // All systems have at least 1 core, maybe more
                }
                return intCores;
            }

            /// <summary>
            /// Type of CPU in computer (i.e. Pentium 4 255Mhz)
            /// </summary>
            static public string GetCPUType()
            {
                return GetProp("Win32_Processor", "Name");
            }

            /// <summary>
            /// Operating system version (i.e. 5.1.0.2600)
            /// </summary>
            static public string GetOSversion()
            {
                return GetProp("Win32_OperatingSystem", "Version");
            }

            /// <summary>
            /// Operating system product code (tells things like OEM versus VLK)
            /// </summary>
            static public string GetOSProductCode()
            {
                string temp = GetProp("Win32_OperatingSystem", "SerialNumber");
                return temp.Substring(0, 9); // return product code and channel ID
            }

            /// <summary>
            /// Operating system name
            /// Check channel ID to see if software is VLK.  Append to title
            /// </summary>
            static public string GetOSName()
            {
                string temp = GetProp("Win32_OperatingSystem", "Caption").Trim();

                // Scan for channel ID (640=VLK, OEM=OEM)
                if (GetOSProductCode().Contains("640")) temp = temp + " VLK";
                else if (GetOSProductCode().Contains("OEM")) temp = temp + " OEM";

                return temp;
            }

            /// <summary>
            /// Operating system installation date
            /// </summary>
            static public string GetOSInstallDate()
            {
                string temp = GetProp("Win32_OperatingSystem", "InstallDate").Trim();
                temp = ManagementDateTimeConverter.ToDateTime(temp).ToShortDateString();

                return temp;
            }

            /// <summary>
            /// Gets the operating system architecture (i.e. x86 or x64)
            /// </summary>
            /// <returns>32-bit or 64-bit</returns>
            static public string GetOSArchitecture()
            {
                // This occasionally fails for unknown reasons!  Not sure why.
                try
                {
                    string temp = GetProp("Win32_OperatingSystem", "OSArchitecture");
                    return (temp);
                }
                catch (Exception)
                {
                    return "unknown";
                }
            }

            /// <summary>
            /// Username of current user without domain.
            /// This is ONLY the name of the user currently running this process.
            /// So if this is called from a Windows service, it will return "SERVICE"
            /// </summary>
            static public string GetCurrentUser()
            {
                string user = "";

                // If this fails for any reason (i.e. no user logged in), return the name anyway
                try
                {
                    // Normalize username to lowercase then remove domain
                    user = GetProp("Win32_ComputerSystem", "UserName").Trim().ToLower();
                    user = user.Substring(user.IndexOf('\\') + 1);
                }
                catch (Exception)
                {
                    return "";
                }
                return user;
            }

            /// <summary>
            /// Returns a list of all installed software.  This is more complete than
            /// the "Software" property.  This function returns the most detailed info.
            /// </summary>
            static public SoftwareInfo[] GetSoftwareInfo()
            {
                List<SoftwareInfo> packages = new List<SoftwareInfo>();

                try
                {
                    // Open registry then iterate through all installed packages for 32-bit registry (i.e. WOW6432)
                    RegistryKey rootReg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                    RegistryKey key = rootReg.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall", false);
                    string[] subkeys = key.GetSubKeyNames();
                    foreach (string subkey in subkeys)
                    {
                        // Open the software's key
                        RegistryKey tempkey = key.OpenSubKey(subkey);
                        // Populate a SoftwareInfo object
                        SoftwareInfo temppackage = new SoftwareInfo();
                        temppackage.ID = subkey;
                        temppackage.Name = (string)tempkey.GetValue("DisplayName", "");
                        temppackage.Version = (string)tempkey.GetValue("DisplayVersion", "");
                        temppackage.Company = (string)tempkey.GetValue("Publisher", "");
                        temppackage.Is64bit = false;
                        // Only add packages with display names and don't add duplicates
                        if (temppackage.Name.Length > 0 && !packages.Contains(temppackage))
                        {
                            packages.Add(temppackage);
                        }
                        tempkey.Close();
                    }
                    key.Close();
                    rootReg.Close();

                    // Open registry then iterate through all installed packages for 64-bit registry
                    if (Environment.Is64BitOperatingSystem)
                    {
                        rootReg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                        key = rootReg.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall", false);
                        subkeys = key.GetSubKeyNames();
                        foreach (string subkey in subkeys)
                        {
                            // Open the software's key
                            RegistryKey tempkey = key.OpenSubKey(subkey);
                            // Populate a SoftwareInfo object
                            SoftwareInfo temppackage = new SoftwareInfo();
                            temppackage.ID = subkey;
                            temppackage.Name = (string)tempkey.GetValue("DisplayName", "");
                            temppackage.Version = (string)tempkey.GetValue("DisplayVersion", "");
                            temppackage.Company = (string)tempkey.GetValue("Publisher", "");
                            temppackage.Is64bit = true;
                            // Only add packages with display names and don't add duplicates
                            if (temppackage.Name.Length > 0 && !packages.Contains(temppackage))
                            {
                                packages.Add(temppackage);
                            }
                            tempkey.Close();
                        }
                        key.Close();
                        rootReg.Close();
                    }

                    // Now we'll be nice and sort everything.  Why?  Just because.
                    packages.Sort();
                }
                catch (Exception)
                {
                }

                // Copy from generic List to array and return it
                SoftwareInfo[] array = new SoftwareInfo[packages.Count];
                packages.CopyTo(array);
                return array;
            }

            /// <summary>
            /// Gets the list of printers physically connected to computer
            /// </summary>
            static public StringCollection GetPrinters()
            {
                StringCollection printers = new StringCollection();

                // Get WMI Class
                ManagementClass theClass = new ManagementClass("Win32_Printer");
                ManagementObjectCollection theInstances;
                theInstances = theClass.GetInstances();

                // Cycle through all printers
                foreach (ManagementObject theInstance in theInstances)
                {
                    // Get printer attributes
                    if ((bool)theInstance.Properties["Local"].Value) // Check if local printer
                    {
                        // Get printer name
                        printers.Add(theInstance.Properties["Caption"].Value.ToString());
                    }
                }

                // Now we'll be nice and sort everything.  Why?  Just because.
                ArrayList.Adapter(printers).Sort();
                return printers;
            }

            /// <summary>
            /// Gets information about locally attached drives
            /// </summary>
            static public DriveInfo[] GetDrives()
            {
                // First, let's count how many fixed drives exist
                int count = 0;
                System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
                foreach (System.IO.DriveInfo drive in drives)
                {
                    // We only care about fixed drives.  Not USB, network or anything
                    // else.
                    if (drive.DriveType == System.IO.DriveType.Fixed) count++;
                }

                // Allocate the array
                DriveInfo[] info = new DriveInfo[count];

                // Save the information for the drives we care about
                int pos = 0;
                foreach (System.IO.DriveInfo drive in drives)
                {
                    // We only care about fixed drives.  Not USB, network or anything
                    // else.
                    if (drive.DriveType == System.IO.DriveType.Fixed)
                    {
                        info[pos] = new DriveInfo();
                        info[pos].DriveLetter = drive.Name[0];
                        info[pos].TotalSize = (int)(drive.TotalSize / Math.Pow(1024, 3));
                        info[pos].FreeSpace = (int)(drive.AvailableFreeSpace / Math.Pow(1024, 3));
                        pos++;
                    }
                }

                // Return information
                return info;
            }

            /// <summary>
            /// Returns the MAC address of the currently active network interface
            /// </summary>
            static public string GetMACAddress()
            {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in adapters)
                {
                    // Check to see if adapter is up.  Otherwise ignore
                    if (adapter.OperationalStatus == OperationalStatus.Up)
                    {
                        return adapter.GetPhysicalAddress().ToString();
                    }
                }

                return ""; // No active adapter.  Return empty
            }

            /// <summary>
            /// Returns the IP address of the currently active network interface
            /// </summary>
            static public string GetIPAddress()
            {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in adapters)
                {
                    // Check to see if adapter is up.  Otherwise ignore
                    if (adapter.OperationalStatus == OperationalStatus.Up)
                    {
                        // An interface might have multiple addresses (rare!)
                        // Return the first found address
                        foreach (UnicastIPAddressInformation info in adapter.GetIPProperties().UnicastAddresses)
                        {
                            // Only return IPv4 addresses.  Ignore IPv6
                            if (info.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                return info.Address.ToString();
                        }
                    }
                }

                return ""; // No active adapter.  Return empty
            }

            /// <summary>
            /// Returns the full computer type:
            /// virtual workstation, virtual server, virtual domain controller
            /// domain controller, laptop, desktop, server,
            /// desktop server, laptop server
            /// </summary>
            /// <returns></returns>
            static public string GetFullComputerType()
            {
                string osType = GetOSType();
                string chassisType = GetComputerType();

                // virtual workstation, virtual server, virtual domain controller
                if (IsVirtualMachine())
                {
                    return "virtual " + osType;
                }

                // domain controller
                else if (osType == "domain controller") return "domain controller";

                // server
                else if (chassisType == "server") return "server";

                // laptop, laptop server, desktop, desktop server
                else
                {
                    string temp = chassisType;
                    // Append server to type if the O/S is a server O/S
                    if (osType != "workstation") temp += " " + osType;
                    return temp;
                }
            }

            /// <summary>
            /// Returns true if the computer is a virtual machine
            /// </summary>
            static public bool IsVirtualMachine()
            {
                // Look at the motherboard product
                if (GetProp("Win32_BaseBoard", "Product").ToLower().Contains("virtual")) return true;

                // Look at computer model
                else if (GetComputerModel().ToLower().Contains("virtual")) return true;

                // Look at computer manufacturer
                else if (GetComputerManufacturer().ToLower().Contains("microsoft")) return true;

                else return false;
            }

            /// <summary>
            /// Returns the type of O/S: workstation, server, domain controller
            /// </summary>
            /// <returns></returns>
            static public string GetOSType()
            {
                string osProductType = GetProp("Win32_OperatingSystem", "ProductType");
                if (osProductType == "1") return "workstation";
                else if (osProductType == "2") return "domain controller";
                else if (osProductType == "3") return "server";
                else return "unknown - " + osProductType;
            }

            /// <summary>
            /// Gets the computer type - Laptop, Desktop, Server
            /// Not 100% accurate but usually ok
            /// Only looks at the chasis type.  Not the O/S.
            /// </summary>
            static public string GetComputerType()
            {
                // Check memory type.  If it is SODIMMS (type 12) then must be laptop.
                if (GetProp("Win32_PhysicalMemory", "FormFactor") == "12") return "laptop";

                // Get chasis type from WMI
                int lastchassis = 0;
                ManagementClass theClass = new ManagementClass("Win32_SystemEnclosure");
                ManagementObjectCollection theInstances = theClass.GetInstances();
                foreach (ManagementObject theInstance in theInstances)
                {
                    UInt16[] chassisArray = (UInt16[])theInstance.Properties["ChassisTypes"].Value;

                    foreach (int chassis in chassisArray)
                    {
                        lastchassis = chassis;
                        // Is a server? (23=rack mount chassis, 5=pizza box, 1=other)
                        if (chassis == 23 || chassis == 5 || chassis == 1) return "server";

                        // Is a laptop? (8=portable, 9=laptop, 10=notebook, 14=subnotebook)
                        if (chassis == 8 || chassis == 9 || chassis == 10 || chassis == 14) return "laptop";

                        // Is a desktop? (3=desktop, 4=lowprofile desktop, 6=minitower, 7=tower, 15=space-saving, 16=lunchbox)
                        if (chassis == 3 || chassis == 4 || chassis == 6 || chassis == 7 || chassis == 15 || chassis == 16) return "desktop";
                    }
                }

                // If unknown chassis, include the chassis number for debugging
                return "unknown-" + lastchassis.ToString();
            }

            /// <summary>
            /// Returns true if a user is logged on.
            /// </summary>
            public static bool IsUserLoggedOn()
            {
                // Detect if explorer is running.  If yes, user is logged on.
                if (Process.GetProcessesByName("explorer").Length > 0) return true;
                else return false; // No explorer running, no users!
            }

            /// <summary>
            /// Gets the username of the currently logged in user (if any)
            /// If there are more than one user (i.e. terminal server) only the
            /// first is returned
            /// An empty string is returned if nobody is logged in
            /// </summary>
            static public string GetCurrentLoggedUser()
            {
                try
                {
                    // If no user, return empty
                    if (!IsUserLoggedOn()) return "none";

                    // Get the process ID of a running explorer.exe
                    int id = Process.GetProcessesByName("explorer")[0].Id;

                    // Use the ID to determine the user via WMI
                    string query = "Select * from Win32_Process Where ProcessID=" + id;
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                    ManagementObjectCollection list = searcher.Get();
                    foreach (ManagementObject obj in list)
                    {
                        string[] owners = new string[] { "" };
                        obj.InvokeMethod("GetOwner", owners);
                        if (owners.Length > 0) return owners[0];
                    }
                }
                catch (Exception)
                {
                    return "unknown";
                }

                return "unknown"; // Shouldn't get here
            }

            /// <summary>
            /// Returns the disk drive write speed in KB/s
            /// </summary>
            static public int GetDriveWriteSpeed()
            {
                // Create a temporary file
                UInt32 size = 1048576 * 10; // 10 MB
                string tempfile = Path.GetTempFileName();
                FileStream file = new FileStream(tempfile, FileMode.Create);

                // Write data to file, not using the cache
                DateTime startTime = DateTime.Now;
                for (UInt32 x = 0; x < size; x++)
                {
                    file.WriteByte(0xff);
                    file.Flush(); // Flush so we're not caching
                }
                DateTime endTime = DateTime.Now;

                // Cleanup
                file.Close(); // Close file
                File.Delete(tempfile); // Delete the temp file

                // Report time difference
                TimeSpan span = endTime - startTime;
                double speed = 10.0 / span.TotalSeconds * 1024.0; // Convert to KB/s from MB/s

                // Round speed to an integer
                return (int)Math.Round(speed);
            }

            /// <summary>
            /// Returns array containing all monitor info (can be multiple)
            /// </summary>
            static public MonitorInfo[] GetMonitors()
            {
                // Get WMI Class
                ManagementClass theClass = new ManagementClass("Win32_DesktopMonitor");

                // Retreive the system instances
                ManagementObjectCollection theInstances = theClass.GetInstances();

                // Create array of monitors (may be 0, 1 or more)
                MonitorInfo[] info = new MonitorInfo[theInstances.Count];
                int i = 0, validTotal = 0;
                foreach (ManagementObject theInstance in theInstances)
                {
                    // Create instance and read PNPDeviceID.  Needed to find info in registry.
                    info[i] = new MonitorInfo();
                    info[i].Name = theInstance.Properties["Name"].Value.ToString().Trim();

                    // Check monitor status.  Old, previously connected monitors may be included as available.  Not sure how to prevent this.
                    string availability = theInstance.Properties["Availability"].Value.ToString();
                    info[i].valid = true;

                    // Open registry and read EDID string about monitor
                    try
                    {
                        info[i].PNPDeviceID = theInstance.Properties["PNPDeviceID"].Value.ToString().Trim();
                        RegistryKey key = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Enum\\" + info[i].PNPDeviceID + "\\Device Parameters\\");
                        Byte[] edid = (Byte[])key.GetValue("EDID");
                        key.Close();

                        // Parse EDID data - This isn't trivial!!
                        // Serial number is immiedately AFTER 00 00 00 FF 00
                        int pos = Find(edid, new byte[] { 00, 00, 00, 0xff, 00 });
                        info[i].SerialNumber = StringInEDID(edid, pos);

                        // Computer name/type is after 00 00 FC 00
                        pos = Find(edid, new byte[] { 00, 00, 0xfc, 00 });
                        info[i].Name = StringInEDID(edid, pos);
                    }
                    catch (Exception) // Exception can be thrown if no serial number (i.e. older monitors)
                    {
                        info[i].valid = false;
                    }

                    if (info[i].valid) validTotal++;
                    i++;
                }

                // Now create a new array with ONLY valid monitors
                MonitorInfo[] validMonitors = new MonitorInfo[validTotal];
                i = 0;
                foreach (MonitorInfo monitor in info)
                {
                    if (monitor.valid)
                    {
                        validMonitors[i] = new MonitorInfo();
                        validMonitors[i].ManufactureWeek = monitor.ManufactureWeek;
                        validMonitors[i].ManufactureYear = monitor.ManufactureYear;
                        validMonitors[i].Name = monitor.Name;
                        validMonitors[i].PNPDeviceID = monitor.PNPDeviceID;
                        validMonitors[i].SerialNumber = monitor.SerialNumber;
                        validMonitors[i].valid = monitor.valid;
                        i++;
                    }
                }

                return validMonitors;
            }
        }
    
}
