using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.VisualBasic;
using XylonV2.ComputerInfo.Define;
using System.Runtime.InteropServices;

namespace StrelyCleaner.Core.Hardware
{
    public class SYSInfoMonitorLib
    {
        // Load all suffixes in an array  
        static  readonly string[] suffixes =
        { "Bytes", "KB", "MB", "GB", "TB", "PB" };

        public static string FormatSize(Int64 bytes)
        {
            int counter = 0;
            decimal number = (decimal)bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }

        public static string StringBuilderFunc(List<KeyValuePair<string, string>> data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var values in data)
            {
                sb.AppendFormat("{0}: {1}", values.Key, values.Value);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static List<KeyValuePair<string, string>> GetProcessorInfoInKeyValuePair()
        {
            var processorInfo = new List<KeyValuePair<string, string>>();
            try {

                ManagementObjectSearcher myProcessorObject = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (ManagementObject obj in myProcessorObject.Get())
                {
                    processorInfo.Add(new KeyValuePair<string, string>("Name", ParseString(obj, "Name")));
                    processorInfo.Add(new KeyValuePair<string, string>("DeviceID", ParseString(obj, "DeviceID")));
                    processorInfo.Add(new KeyValuePair<string, string>("Manufacturer", ParseString(obj, "Manufacturer")));

                    processorInfo.Add(new KeyValuePair<string, string>("Caption", ParseString(obj, "Caption")));
                    processorInfo.Add(new KeyValuePair<string, string>("NumberOfCores", ParseString(obj, "NumberOfCores")));
                    processorInfo.Add(new KeyValuePair<string, string>("NumberOfLogicalProcessors", ParseString(obj, "NumberOfLogicalProcessors")));
                    processorInfo.Add(new KeyValuePair<string, string>("VirtualizationFirmwareEnabled", ParseString(obj, "VirtualizationFirmwareEnabled")));
                    processorInfo.Add(new KeyValuePair<string, string>("Architecture", ParseString(obj, "Architecture")));
                    processorInfo.Add(new KeyValuePair<string, string>("Family", ParseString(obj, "Family")));
                    processorInfo.Add(new KeyValuePair<string, string>("ProcessorType", ParseString(obj, "ProcessorType")));
                    try
                    {
                        processorInfo.Add(new KeyValuePair<string, string>("Characteristics", $"{obj["Characteristics"]}"));
                    }
                    catch (Exception)
                    {
                        processorInfo.Add(new KeyValuePair<string, string>("Characteristics", "Not Found"));
                    }


                    processorInfo.Add(new KeyValuePair<string, string>("AddressWidth", $"{ParseString(obj, "AddressWidth")}bit"));

                    try
                    {
                        processorInfo.Add(new KeyValuePair<string, string>("CurrentClockSpeed", $"{float.Parse(ParseString(obj, "CurrentClockSpeed")) / 1000}Ghz"));
                    }
                    catch { }
                }

            } catch { }
           
            return processorInfo;
        }

        private static string ParseString(ManagementObject obj, string ID, string DefValue = "") {
            try {
                var Result = $"{obj[ID]}";
                if (Result != null) { return Result.ToString(); } else { return DefValue; }
            } catch { return DefValue; }
        }


        public sealed class RAM
        {
            public string BankLabel { get; set; }
            public ByteSize Capacity { get; set; }
            public string FormFactor { get; set; }
            public string Manufacturer { get; set; }
            public string MemoryType { get; set; }
            public UInt32 Speed { get; set; }
        }

        public static List<RAM> GetRAM()
        {
            List<RAM> modules = new List<RAM>();

             try {

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");

                foreach (ManagementObject mo in searcher.Get())
                {
                    RAM module = new RAM();

                    try { module.BankLabel = Convert.ToString(mo.Properties["BankLabel"].Value); } catch { }
                    try { module.Capacity = ByteSize.FromBytes(Convert.ToDouble(mo.Properties["Capacity"].Value)); } catch { }
                    try { module.Manufacturer = Convert.ToString(mo.Properties["Manufacturer"].Value); } catch { }
                    try { module.Speed = Convert.ToUInt32(mo.Properties["Speed"].Value); } catch { }
                    try { UInt16 memorytype = Convert.ToUInt16(mo.Properties["MemoryType"].Value); module.MemoryType = SanitizeMemoryType((int)memorytype); ; } catch { }
                    try { UInt16 formfactor = Convert.ToUInt16(mo.Properties["FormFactor"].Value); module.FormFactor = SanitizeFormFactor(formfactor); } catch { }


                    modules.Add(module);
                }

            } catch { }

             

            return modules;
        }


        private static string SanitizeFormFactor(UInt16 i)
        {
            string result = string.Empty;

            switch (i)
            {
                case 0:
                    result = "Unknown";
                    break;
                case 1:
                    result = "Other";
                    break;
                case 2:
                    result = "SIP";
                    break;
                case 3:
                    result = "DIP";
                    break;
                case 4:
                    result = "ZIP";
                    break;
                case 5:
                    result = "SOJ";
                    break;
                case 6:
                    result = "Proprietary";
                    break;
                case 7:
                    result = "SIMM";
                    break;
                case 8:
                    result = "DIMM";
                    break;
                case 9:
                    result = "TSOP";
                    break;
                case 10:
                    result = "PGA";
                    break;
                case 11:
                    result = "RIMM";
                    break;
                case 12:
                    result = "SODIMM";
                    break;
                case 13:
                    result = "SRIMM";
                    break;
                case 14:
                    result = "SMD";
                    break;
                case 15:
                    result = "SSMP";
                    break;
                case 16:
                    result = "QFP";
                    break;
                case 17:
                    result = "TQFP";
                    break;
                case 18:
                    result = "SOIC";
                    break;
                case 19:
                    result = "LCC";
                    break;
                case 20:
                    result = "PLCC";
                    break;
                case 21:
                    result = "BGA";
                    break;
                case 22:
                    result = "FPBGA";
                    break;
                case 23:
                    result = "LGA";
                    break;
                default:
                    result = ($"Unknown ({i})");
                    break;
            }

            return result;
        }


        public enum MemoryType
        {
            Unknown = 0,
            Other = 1,
            DRAM = 2,
            SynchronousDRAM = 3,
            CacheDRAM = 4,
            EDO = 5,
            EDRAM = 6,
            VRAM = 7,
            SRAM = 8,
            RAM = 9,
            ROM = 10,
            Flash = 11,
            EEPROM = 12,
            FEPROM = 13,
            EPROM = 14,
            CDRAM = 15,
            _3DRAM = 16,
            SDRAM = 17,
            SGRAM = 18,
            RDRAM = 19,
            DDR = 20,
            DDR2 = 21,
            DDR2FB = 22,
            DDR2FC = 23,
            Reserved24 = 24,
            Reserved25 = 25,
            Reserved26 = 26,
            Reserved27 = 27,
            Reserved28 = 28,
            Reserved29 = 29,
            Reserved30 = 30,
            DDR3 = 31,
            FBD2 = 32,
            DDR4 = 33,
            LPDDR = 34,
            LPDDR2 = 35,
            LPDDR3 = 36,
            LPDDR4 = 37,
            LPDDR4x = 38,
            LPDDR5 = 39,
            LPDDR5x = 40,
            DDR5 = 41
        }


        private static string SanitizeMemoryType(int i)
        {
            string result = string.Empty;

            if (Enum.IsDefined(typeof(MemoryType), i))
            {
                MemoryType memoryTypeEnum = (MemoryType)i;
                result = memoryTypeEnum.ToString();
            }
            else
            {
                result = ($"Unknown ({i})");
            }

            return result;
        }
        private static string SanitizeDeviceInterface(UInt16 i)
        {
            string result = string.Empty;

            switch (i)
            {
                case 1:
                    result = "Other";
                    break;
                case 2:
                    result = "Unknown";
                    break;
                case 3:
                    result = "Serial";
                    break;
                case 4:
                    result = "PS/2";
                    break;
                case 5:
                    result = "Infrared";
                    break;
                case 6:
                    result = "HP-HIL";
                    break;
                case 7:
                    result = "Bus mouse";
                    break;
                case 8:
                    result = "ADB (Apple Desktop Bus)";
                    break;
                case 160:
                    result = "Bus mouse DB-9";
                    break;
                case 161:
                    result = "Bus mouse micro-DIN";
                    break;
                case 162:
                    result = "USB";
                    break;
            }

            return result;
        }

        private static string SanitizePointingType(UInt16 i)
        {
            string result = string.Empty;

            switch (i)
            {
                case 1:
                    result = "Other";
                    break;
                case 2:
                    result = "Unknown";
                    break;
                case 3:
                    result = "Mouse";
                    break;
                case 4:
                    result = "Track Ball";
                    break;
                case 5:
                    result = "Track Point";
                    break;
                case 6:
                    result = "Glide Point";
                    break;
                case 7:
                    result = "Touch Pad";
                    break;
                case 8:
                    result = "Touch Screen";
                    break;
                case 9:
                    result = "Mouse - Optical Sensor";
                    break;
            }

            return result;
        }

        public static List<KeyValuePair<string, string>> GetGraphicsInfo()
        {
            var GraphicsInfo = new List<KeyValuePair<string, string>>();
            try {

                ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_VideoController");

                foreach (ManagementObject obj in myVideoObject.Get())
                {
                    GraphicsInfo.Add(new KeyValuePair<string, string>("Name", ParseString(obj, "Name")));
                    GraphicsInfo.Add(new KeyValuePair<string, string>("Status", ParseString(obj, "Status")));
                    GraphicsInfo.Add(new KeyValuePair<string, string>("Caption", ParseString(obj, "Caption")));
                    GraphicsInfo.Add(new KeyValuePair<string, string>("DeviceID", ParseString(obj, "DeviceID")));
                    GraphicsInfo.Add(new KeyValuePair<string, string>("AdapterRAM", $"{FormatSize(Convert.ToInt64(ParseString(obj, "AdapterRAM", "0")))}"));
                    GraphicsInfo.Add(new KeyValuePair<string, string>("AdapterDACType", ParseString(obj, "AdapterDACType")));
                    GraphicsInfo.Add(new KeyValuePair<string, string>("Monochrome", ParseString(obj, "Monochrome")));
                    GraphicsInfo.Add(new KeyValuePair<string, string>("InstalledDisplayDrivers", ParseString(obj, "InstalledDisplayDrivers")));
                    GraphicsInfo.Add(new KeyValuePair<string, string>("DriverVersion", ParseString(obj, "DriverVersion")));
                    GraphicsInfo.Add(new KeyValuePair<string, string>("VideoProcessor", ParseString(obj, "VideoProcessor")));
                    GraphicsInfo.Add(new KeyValuePair<string, string>("VideoArchitecture", ParseString(obj, "VideoArchitecture")));
                    GraphicsInfo.Add(new KeyValuePair<string, string>("VideoMemoryType", ParseString(obj, "VideoMemoryType")));
                    GraphicsInfo.Add(new KeyValuePair<string, string>(" ", " "));
                }

            } catch { }
           
            return GraphicsInfo;
        }

        public static List<KeyValuePair<string, string>> GetStorageInfo()
        {
            var StorageInfo = new List<KeyValuePair<string, string>>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                // Console.WriteLine("Drive {0}", d.Name);

                if (d.IsReady == true)
                {
                    StorageInfo.Add(new KeyValuePair<string, string>("DriveName", $"{d.Name}"));
                    if (d.Name.ToLower().Contains("c"))
                    {
                        StorageInfo.Add(new KeyValuePair<string, string>("sysdrive", $"{FormatSize(Convert.ToInt64(d.AvailableFreeSpace))} of {FormatSize(Convert.ToInt64(d.TotalSize))} Available"));
                    }
                    StorageInfo.Add(new KeyValuePair<string, string>("DriveInfo", $"{d.DriveType}"));
                    StorageInfo.Add(new KeyValuePair<string, string>("VolumeLabel", $"{d.VolumeLabel}"));
                    StorageInfo.Add(new KeyValuePair<string, string>("DriveFormat", $"{d.DriveFormat}"));
                    StorageInfo.Add(new KeyValuePair<string, string>("AvailableFreeSpace", $"{FormatSize(Convert.ToInt64(d.AvailableFreeSpace))}"));
                    StorageInfo.Add(new KeyValuePair<string, string>("TotalSize", $"{FormatSize(Convert.ToInt64(d.TotalSize))}"));
                    StorageInfo.Add(new KeyValuePair<string, string>("RootDirectory", $"{d.RootDirectory}"));
                    StorageInfo.Add(new KeyValuePair<string, string>(" ", " "));
                }
            }
            return StorageInfo;
        }
        public static List<KeyValuePair<string, string>> GetOSInfo()
        {
            var OSInfo = new List<KeyValuePair<string, string>>();

            ManagementObjectSearcher myOperativeSystemObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");

            foreach (ManagementObject obj in myOperativeSystemObject.Get())
            {
                OSInfo.Add(new KeyValuePair<string, string>("Name", ParseString(obj, "Caption")));
                OSInfo.Add(new KeyValuePair<string, string>("OSType", ParseString(obj, "OSType") ));
                OSInfo.Add(new KeyValuePair<string, string>("Version", ParseString(obj, "Version") ));
                OSInfo.Add(new KeyValuePair<string, string>("WindowsDirectory", ParseString(obj, "WindowsDirectory") ));
                OSInfo.Add(new KeyValuePair<string, string>("ProductType", ParseString(obj, "ProductType") ));
                OSInfo.Add(new KeyValuePair<string, string>("SerialNumber", ParseString(obj, "SerialNumber") ));
                OSInfo.Add(new KeyValuePair<string, string>("SystemDirectory", ParseString(obj, "SystemDirectory") ));
                OSInfo.Add(new KeyValuePair<string, string>("CountryCode", ParseString(obj, "CountryCode") ));
                OSInfo.Add(new KeyValuePair<string, string>("CurrentTimeZone", ParseString(obj, "CurrentTimeZone")));
                OSInfo.Add(new KeyValuePair<string, string>("EncryptionLevel",  $"{ParseString(obj, "EncryptionLevel")}-bit Encryption"));
            }
            return OSInfo;
        }

        public static List<KeyValuePair<string, string>> GetNetworkInformation()
        {
            var NetworkInformation = new List<KeyValuePair<string, string>>();

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            if (nics == null || nics.Length < 1)
            {
                NetworkInformation.Add(new KeyValuePair<string, string>("Error", "No Network Interfaces Found!"));
            }
            else
            {
                foreach (NetworkInterface adapter in nics)
                {
                    string versions = "";
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                    {
                        versions = "IPv4";
                    }
                    if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                    {
                        if (versions.Length > 0)
                        {
                            versions += ", ";
                        }
                        versions += "IPv6";
                    }
                    IPInterfaceProperties properties = adapter.GetIPProperties();

                    NetworkInformation.Add(new KeyValuePair<string, string>("Description", $"{adapter.Description}"));
                    NetworkInformation.Add(new KeyValuePair<string, string>("NetworkInterfaceType", $"{adapter.NetworkInterfaceType}"));
                    NetworkInformation.Add(new KeyValuePair<string, string>("PhysicalAddress", $"{adapter.GetPhysicalAddress().ToString()}"));
                    NetworkInformation.Add(new KeyValuePair<string, string>("IPVersion", $"{versions}"));
                    NetworkInformation.Add(new KeyValuePair<string, string>("OperationalStatus", $"{adapter.OperationalStatus}"));
                    NetworkInformation.Add(new KeyValuePair<string, string>(" ", $" "));
                }
            }
            return NetworkInformation;
        }
        public static List<KeyValuePair<string, string>> GetAudioDevices()
        {
            var AudioDevices = new List<KeyValuePair<string, string>>();

            ManagementObjectSearcher myAudioObject = new ManagementObjectSearcher("select * from Win32_SoundDevice");

            foreach (ManagementObject obj in myAudioObject.Get())
            {
                AudioDevices.Add(new KeyValuePair<string, string>("Name", $"{obj["Name"]}"));
                AudioDevices.Add(new KeyValuePair<string, string>("ProductName", $"{obj["ProductName"]}"));
                AudioDevices.Add(new KeyValuePair<string, string>("Availability", $"{obj["Availability"]}"));
                AudioDevices.Add(new KeyValuePair<string, string>("DeviceID", $"{obj["DeviceID"]}"));
                AudioDevices.Add(new KeyValuePair<string, string>("PowerManagementSupported", $"{obj["PowerManagementSupported"]}"));
                AudioDevices.Add(new KeyValuePair<string, string>("Status", $"{obj["Status"]}"));
                AudioDevices.Add(new KeyValuePair<string, string>("StatusInfo", $"{obj["StatusInfo"]}"));
                AudioDevices.Add(new KeyValuePair<string, string>(" ", $" "));

            }
            return AudioDevices;
        }

        public static List<KeyValuePair<string, string>> GetPrinters()
        {
            var Printers = new List<KeyValuePair<string, string>>();

            ManagementObjectSearcher myPrinterObject = new ManagementObjectSearcher("select * from Win32_Printer");

            foreach (ManagementObject obj in myPrinterObject.Get())
            {
                Printers.Add(new KeyValuePair<string, string>("Name", $"{obj["Name"]}"));
                Printers.Add(new KeyValuePair<string, string>("Network", $"{obj["Network"]}"));
                Printers.Add(new KeyValuePair<string, string>("Availability", $"{obj["Availability"]}"));
                Printers.Add(new KeyValuePair<string, string>("Default", $"{obj["Default"]}"));
                Printers.Add(new KeyValuePair<string, string>("DeviceID", $"{obj["DeviceID"]}"));
                Printers.Add(new KeyValuePair<string, string>("Status", $"{obj["Status"]}"));
                Printers.Add(new KeyValuePair<string, string>(" ", $" "));

            }
            return Printers;
        }

        public static List<KeyValuePair<string, string>> GetBaseBoard()
        {
            var BaseBoard = new List<KeyValuePair<string, string>>();
            try {
                ManagementObjectSearcher myPrinterObject = new ManagementObjectSearcher("select * from Win32_BaseBoard");

                foreach (ManagementObject obj in myPrinterObject.Get())
                {
                    BaseBoard.Add(new KeyValuePair<string, string>("Name", ParseString(obj, "Name")));
                    BaseBoard.Add(new KeyValuePair<string, string>("Description", ParseString(obj, "Description")));
                    BaseBoard.Add(new KeyValuePair<string, string>("Manufacturer", ParseString(obj, "Manufacturer")));
                    BaseBoard.Add(new KeyValuePair<string, string>("Model", ParseString(obj, "Model")));
                    BaseBoard.Add(new KeyValuePair<string, string>("Removable", ParseString(obj, "Removable")));
                    BaseBoard.Add(new KeyValuePair<string, string>("SerialNumber", ParseString(obj, "SerialNumber")));
                    BaseBoard.Add(new KeyValuePair<string, string>("Status", ParseString(obj, "Status")));
                    BaseBoard.Add(new KeyValuePair<string, string>("Version", ParseString(obj, "Version")));

                }
            } catch { }
           
            return BaseBoard;
        }

        public static  string DateTimeToString(String DateTime)
        {
            DateTime dt = ManagementDateTimeConverter.ToDateTime(DateTime);
            return dt.ToString("dd-MMM-yyyy");//date format
        }

        public static  List<KeyValuePair<string, string>> GetBIOS()
        {
            var BIOS = new List<KeyValuePair<string, string>>();
            try {
                ManagementObjectSearcher myPrinterObject = new ManagementObjectSearcher("select * from Win32_BIOS");

                foreach (ManagementObject obj in myPrinterObject.Get())
                {
                    BIOS.Add(new KeyValuePair<string, string>("Name", ParseString(obj, "Name")));
                    BIOS.Add(new KeyValuePair<string, string>("Manufacturer", ParseString(obj, "Manufacturer")));
                    BIOS.Add(new KeyValuePair<string, string>("ReleaseDate", $"{DateTimeToString(ParseString(obj, "ReleaseDate", "0/0/0").ToString())}"));
                    BIOS.Add(new KeyValuePair<string, string>("Description", ParseString(obj, "Description")));
                    BIOS.Add(new KeyValuePair<string, string>("SerialNumber", ParseString(obj, "SerialNumber")));
                    BIOS.Add(new KeyValuePair<string, string>("SMBIOSBIOSVersion", ParseString(obj, "SMBIOSBIOSVersion")));
                    BIOS.Add(new KeyValuePair<string, string>("Status", ParseString(obj, "Status")));
                    BIOS.Add(new KeyValuePair<string, string>("Version", ParseString(obj, "Version")));

                }
            } catch { }
           
            return BIOS;
        }

        public static string GetAllActiveNICs()
        {
            string NICs = string.Empty;
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                    if (addr != null)
                    {
                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            NICs = ni.Name + "," + ni.Description;
                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    NICs += "," + ip.Address + "," + ip.IPv4Mask;
                                }
                            }
                        }
                    }
                }
            }
            return NICs;
        }
    }
}
