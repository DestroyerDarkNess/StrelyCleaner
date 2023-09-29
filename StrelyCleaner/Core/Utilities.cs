
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static XylonV2.Core.Helper.PrivilegesManager;

namespace StrelyCleaner.Core
{
    public class Utilities
    {
        public static Bitmap Resize_Image(Image img, Int32 Width, Int32 Height)
        {
            Bitmap Bitmap_Source = new Bitmap(img);
            Bitmap Bitmap_Dest = new Bitmap(System.Convert.ToInt32(Width), System.Convert.ToInt32(Height));
            Graphics Graphic = Graphics.FromImage(Bitmap_Dest);
            Graphic.DrawImage(Bitmap_Source, 0, 0, Bitmap_Dest.Width + 1, Bitmap_Dest.Height + 1);
            return Bitmap_Dest;
        }
        public static string AddDoubleQuotes( string value)
        {
            return "\"" + value + "\"";
        }
        public static bool IsSystem(string FilePath)
        {
            try
            {
                if (FilePath == null)
                    return true;
                bool CheckWindows = (FilePath.Split(@"\".ToCharArray().FirstOrDefault())[1].ToUpper() == "Windows".ToUpper());
                bool CheckAppWin = (FilePath.Split(@"\".ToCharArray().FirstOrDefault())[2].ToUpper() == "WindowsApps".ToUpper());
                bool CheckRecent = (System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetDirectoryName(FilePath)).ToUpper() == "Recent".ToUpper());
                if (CheckWindows == true)
                    return true;
                else if (CheckAppWin == true)
                    return true;
                else if (CheckRecent == true)
                    return true;
                else
                    return false;
            }
            catch { return false; }

        }

        public static bool IsPotencialRiskFormat(string Filename)
        {
            try
            {
                switch (Filename.Split(".".ToCharArray().FirstOrDefault()).LastOrDefault().ToUpper())
                {
                    case "APK":
                        {
                            return true;
                        }

                    case "APP":
                        {
                            return true;
                        }

                    case "BAT":
                        {
                            return true;
                        }

                    case "BIN":
                        {
                            return true;
                        }

                    case "CMD":
                        {
                            return true;
                        }

                    case "COM":
                        {
                            return true;
                        }

                    case "COMMAND":
                        {
                            return true;
                        }

                    case "CPL":
                        {
                            return true;
                        }

                    case "CSH":
                        {
                            return true;
                        }

                    case "EXE":
                        {
                            return true;
                        }

                    case "JS":
                        {
                            return true;
                        }

                    case "PY":
                        {
                            return true;
                        }

                    case "GADGET":
                        {
                            return true;
                        }

                    case "INF1":
                        {
                            return true;
                        }

                    case "INS":
                        {
                            return true;
                        }

                    case "INX":
                        {
                            return true;
                        }

                    case "IPA":
                        {
                            return true;
                        }

                    case "ISU":
                        {
                            return true;
                        }

                    case "JOB":
                        {
                            return true;
                        }

                    case "JSE":
                        {
                            return true;
                        }

                    case "KSH":
                        {
                            return true;
                        }

                    case "LNK":
                        {
                            return true;
                        }

                    case "MSC":
                        {
                            return true;
                        }

                    case "MSI":
                        {
                            return true;
                        }

                    case "MSP":
                        {
                            return true;
                        }

                    case "MST":
                        {
                            return true;
                        }

                    case "OSX":
                        {
                            return true;
                        }

                    case "OUT":
                        {
                            return true;
                        }

                    case "PAF":
                        {
                            return true;
                        }

                    case "PIF":
                        {
                            return true;
                        }

                    case "PRG":
                        {
                            return true;
                        }

                    case "PS1":
                        {
                            return true;
                        }

                    case "REG":
                        {
                            return true;
                        }

                    case "RGS":
                        {
                            return true;
                        }

                    case "RUN":
                        {
                            return true;
                        }

                    case "SCR":
                        {
                            return true;
                        }

                    case "SCT":
                        {
                            return true;
                        }

                    case "SHB":
                        {
                            return true;
                        }

                    case "SHS":
                        {
                            return true;
                        }

                    case "U3P":
                        {
                            return true;
                        }

                    case "VBE":
                        {
                            return true;
                        }

                    case "VBS":
                        {
                            return true;
                        }

                    case "VBSCRIPT":
                        {
                            return true;
                        }

                    case "WORKFLOW":
                        {
                            return true;
                        }

                    case "WS":
                        {
                            return true;
                        }

                    case "WSF":
                        {
                            return true;
                        }

                    case "WSH":
                        {
                            return true;
                        }

                    case "HTA":
                        {
                            return true;
                        }
                    case "ZIP":
                        {
                            return true;
                        }
                    case "RAR":
                        {
                            return true;
                        }
                    case "7Z":
                        {
                            return true;
                        }
                    case "PDF":
                        {
                            return true;
                        }

                    default:
                        {
                            return false;
                        }
                }
            }
            catch
            {
                return false;
            }
        }

        #region " My Application Is Already Running Function "

        // [ My Application Is Already Running Function ]
        // 
        // // By Elektro H@cker
        // 
        // Examples :
        // MsgBox(My_Application_Is_Already_Running)
        // If My_Application_Is_Already_Running() Then Application.Exit()

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern int CreateMutexA(int lpSecurityAttributes, bool bInitialOwner, string lpName);
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern int GetLastError();

        public static bool My_Application_Is_Already_Running()
        {
            // Attempt to create defualt mutex owned by process
            CreateMutexA(0, true, Process.GetCurrentProcess().MainModule.ModuleName.ToString());
            return (GetLastError() == 183); // 183 = ERROR_ALREADY_EXISTS
        }

        #endregion



        public static bool IsScriptFormat(string Filename)
        {
            try
            {
                switch (Filename.Split(".".ToCharArray().FirstOrDefault()).LastOrDefault().ToUpper())
                {
                    case "BAT":
                        {
                            return true;
                        }

                    case "CMD":
                        {
                            return true;
                        }

                    case "VBS":
                        {
                            return true;
                        }

                    case "WSF":
                        {
                            return true;
                        }

                    case "JS":
                        {
                            return true;
                        }

                    case "PS1":
                        {
                            return true;
                        }

                    case "HTA":
                        {
                            return true;
                        }

                    case "LNK":
                        {
                            return true;
                        }
                    case "SCR":
                        {
                            return true;
                        }
                    case "COM":
                        {
                            return true;
                        }
                    default:
                        {
                            return false;
                        }
                }
            }
            catch
            {
                return false;
            }
        }

        public static double GetGPUUsagePercentage()
        {
            try
            {
                double gpuUsage = -1.0; // Valor predeterminado en caso de error

                // Consultar la información de rendimiento de la GPU utilizando WMI
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PerfFormattedData_PerfProc_GPU");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    gpuUsage = Convert.ToDouble(queryObj["PercentProcessorTime"]);
                    break; // Salir después de la primera iteración, ya que generalmente hay un solo resultado
                }

                return gpuUsage;
            }
            catch //(ManagementException ex)
            {
                //Console.WriteLine($"Error al obtener el uso de la GPU: {ex.Message}");
                return -1.0; // Valor predeterminado en caso de error
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        public static Process GetForegroundProcess()
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            uint processId;
            GetWindowThreadProcessId(foregroundWindow, out processId);

            try
            {
                return Process.GetProcessById((int)processId);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public static readonly HashSet<string> EssentialProcessNames = new HashSet<string>
    {
        "System",        // System
        "smss",          // Session Manager Subsystem
        "csrss",         // Client Server Runtime Process
        "wininit",       // Windows Start-Up Application
        "services",      // Services Control Manager
        "lsass",         // Local Security Authority Subsystem Service
        "svchost",       // Service Host
        "winlogon",      // Windows Logon Application
        "spoolsv",       // Print Spooler
        "explorer",      // Windows Explorer
        "lsass",         // Local Security Authority Subsystem Service
        "dwm",           // Desktop Window Manager
        "fontdrvhost",   // Font Driver Host
        "taskhostw",     // Host Process for Windows Tasks
        "SystemSettings",// System Settings
    };

        public static bool IsEssentialProcess(string processName)
        {
            return EssentialProcessNames.Contains(processName, StringComparer.OrdinalIgnoreCase);
        }
        public static bool IsProcessWithoutWindow(int processId)
        {
            try
            {
                Process process = Process.GetProcessById(processId);
                return process.MainWindowHandle == IntPtr.Zero;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public static bool IsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        #region " Sleep "

        // [ Sleep ]
        // 
        // // By Elektro H@cker
        // 
        // Examples :
        // Sleep(5) : MsgBox("Test")
        // Sleep(5, Measure.Seconds) : MsgBox("Test")

        public enum Measure
        {
            Milliseconds = 1,
            Seconds = 2,
            Minutes = 3,
            Hours = 4
        }

        public static void Sleep(Int64 Duration, Measure Measure = Measure.Seconds)
        {
            var Starttime = DateTime.Now;

            switch (Measure)
            {
                case Measure.Milliseconds:
                    {
                        while ((DateTime.Now - Starttime).TotalMilliseconds < Duration)
                            Application.DoEvents();
                        break;
                    }

                case Measure.Seconds:
                    {
                        while ((DateTime.Now - Starttime).TotalSeconds < Duration)
                            Application.DoEvents();
                        break;
                    }

                case Measure.Minutes:
                    {
                        while ((DateTime.Now - Starttime).TotalMinutes < Duration)
                            Application.DoEvents();
                        break;
                    }

                case Measure.Hours:
                    {
                        while ((DateTime.Now - Starttime).TotalHours < Duration)
                            Application.DoEvents();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        #endregion

        #region " Runner "

        public static bool RunCommand(string command, bool WaitForExit = false)
        {
            using (Process p = new Process())
            {
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/C " + command;

                try
                {
                    p.Start();

                    if (WaitForExit == true)
                    {
                        p.WaitForExit();
                        p.Close();
                    }

                    return true;
                }
                catch /*(Exception ex)*/
                {
                    return false;
                }
            }
        }

        public static string RunConsole(string Filename, string Args = "")
        {
            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.FileName = Filename;
                p.StartInfo.Arguments = Args;
                p.StartInfo.RedirectStandardOutput = true;

                try
                {
                    p.Start();
                    string StandardOutput = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    p.Close();
                    return StandardOutput;
                }
                catch /*(Exception ex)*/
                {
                    return string.Empty;
                }
            }
        }


        #endregion

        #region " Services "

        public static void StopService(string serviceName)
        {
            if (ServiceExists(serviceName))
            {
                ServiceController sc = new ServiceController(serviceName);
                if (sc.CanStop)
                {
                    sc.Stop();
                }
            }
        }

        public static bool ServiceExists(string serviceName)
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(serviceName));
        }

        public static bool ServiceIsRuning(string serviceName)
        {
            foreach (var service in ServiceController.GetServices())
            {
                if (service.ServiceName.Equals(serviceName) == true)
                {
                    return (service.Status == ServiceControllerStatus.Running);
                }
            }
            return false;
        }

        public static void StartService(string serviceName)
        {
            if (ServiceExists(serviceName))
            {
                ServiceController sc = new ServiceController(serviceName);

                try
                {
                    sc.Start();
                }
                catch /*(Exception ex)*/
                {

                }
            }
        }

        #endregion

        // Usage:
        // Attrib("File.txt", IO.FileAttributes.ReadOnly + IO.FileAttributes.Hidden)
        // If Attrib("File.txt", IO.FileAttributes.System) Is Nothing Then MsgBox("File doesn't exist!")
        public static bool Attrib(string File, System.IO.FileAttributes Attributes)
        {
            if (System.IO.File.Exists(File))
            {
                try
                {
                    System.IO.File.SetAttributes(File, Attributes);
                    return true; // File was modified OK
                }
                catch
                {
                    // MsgBox(ex.Message)
                    return false;
                }// File can't be modified maybe because User Permissions
            }
            else
                return false;// File doesn't exist
        }


        internal static void TryDeleteRegistryValue(bool localMachine, string path, string valueName)
        {
            try
            {
                if (localMachine) Registry.LocalMachine.OpenSubKey(path, true)?.DeleteValue(valueName, false);
                if (!localMachine) Registry.CurrentUser.OpenSubKey(path, true)?.DeleteValue(valueName, false);
            }
            catch { }
        }

        public static bool CreateRegistryKeyIfNotExists(RegistryHive hive, string subKey)
        {
            using (var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default))
            using (var key = baseKey.OpenSubKey(subKey))
            {
                if (key == null)
                {
                    try
                    {
                        using (var newKey = baseKey.CreateSubKey(subKey))
                        {

                        }
                        return true;
                    }
                    catch 
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }


        }
    }
}
