using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Tweats
{
    internal class Defender : Interfaces.ITweat
    {
        public string id => "Defender";

        public string Description => "Turn off Unnecessary Windows Defender features that affect performance.";

        public void Disabled()
        {
            EnableDefender();
        }

        public void Optimize()
        {
            DisableDefender();
        }

        public bool Get()
        {
            try {
                string A = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiVirus", string.Empty).ToString();
                string B = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableSpecialRunningModes", string.Empty).ToString();
                string C = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableRoutinelyTakingAction", string.Empty).ToString();
                string D = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "ServiceKeepAlive", string.Empty).ToString();
                string E = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableRealtimeMonitoring", string.Empty).ToString();

                if (A != "1" || B != "1" || C != "1" || E != "1")
                {
                    return false;
                }
                else { return true; }
            } catch { return false; }
        }

        internal static void DisableDefender()
        {
            try
            {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiVirus", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableSpecialRunningModes", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableRoutinelyTakingAction", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "ServiceKeepAlive", "0", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableRealtimeMonitoring", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Signature Updates", "ForceUpdateFromMU", 0);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Spynet", "DisableBlockAtFirstSeen", 1);

                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus", "0", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "PUAProtection", "0", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Policy Manager", "DisableScanningNetworkFiles", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableRealtimeMonitoring", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows Defender\Spynet", "SpyNetReporting", "0", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows Defender\Spynet", "SubmitSamplesConsent", "0", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\MRT", "DontReportInfectionInformation", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\MRT", "DontOfferThroughWUAU", "1", RegistryValueKind.DWord);
                Registry.ClassesRoot.DeleteSubKeyTree(@"\CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}", false);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableBehaviorMonitoring", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableOnAccessProtection", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableScanOnRealtimeEnable", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableIOAVProtection", "1", RegistryValueKind.DWord);

                RegistryKey k = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);

                using (RegistryKey tmp = k.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    tmp.DeleteValue("WindowsDefender", false);
                    tmp.DeleteValue("SecurityHealth", false);
                }

                string rootPath;
                if (Environment.Is64BitOperatingSystem)
                {
                    rootPath = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
                }
                else
                {
                    rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                }

                StopService();

                //Utilities.RunCommand(@"regsvr32 /u /s """ + rootPath + "\"");
                //Utilities.RunCommand("Gpupdate /Force");
            }
            catch {  }
        }

        internal static void EnableDefender()
        {
            try {
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\MpEngine", "MpEnablePus", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender", "PUAProtection", "1", RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Policy Manager", "DisableScanningNetworkFiles", "0", RegistryValueKind.DWord);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true).DeleteValue("DisableRealtimeMonitoring", false);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true).DeleteValue("DisableAntiSpyware", false);
                Registry.LocalMachine.OpenSubKey(@"Software\Policies\Microsoft\Windows Defender\Spynet", true).DeleteValue("SpyNetReporting", false);
                Registry.LocalMachine.OpenSubKey(@"Software\Policies\Microsoft\Windows Defender\Spynet", true).DeleteValue("SubmitSamplesConsent", false);
                Registry.LocalMachine.OpenSubKey(@"Software\Policies\Microsoft\MRT", true).DeleteValue("DontReportInfectionInformation", false);
                Registry.LocalMachine.OpenSubKey(@"Software\Policies\Microsoft\MRT", true).DeleteValue("DontOfferThroughWUAU", false);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", true).DeleteValue("DisableBehaviorMonitoring", false);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", true).DeleteValue("DisableOnAccessProtection", false);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", true).DeleteValue("DisableScanOnRealtimeEnable", false);
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", true).DeleteValue("DisableIOAVProtection", false);

                Utilities.TryDeleteRegistryValue(true, @"SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiVirus");
                Utilities.TryDeleteRegistryValue(true, @"SOFTWARE\Policies\Microsoft\Windows Defender", "DisableSpecialRunningModes");
                Utilities.TryDeleteRegistryValue(true, @"SOFTWARE\Policies\Microsoft\Windows Defender", "DisableRoutinelyTakingAction");
                Utilities.TryDeleteRegistryValue(true, @"SOFTWARE\Policies\Microsoft\Windows Defender", "ServiceKeepAlive");
                Utilities.TryDeleteRegistryValue(true, @"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableRealtimeMonitoring");
                Utilities.TryDeleteRegistryValue(true, @"SOFTWARE\Policies\Microsoft\Windows Defender\Signature Updates", "ForceUpdateFromMU");
                Utilities.TryDeleteRegistryValue(true, @"SOFTWARE\Policies\Microsoft\Windows Defender\Spynet", "DisableBlockAtFirstSeen");

                //Utilities.RunCommand("Gpupdate /Force");
            }
            catch { }
        }

        public static void StopService()
        {
           
                string serviceNamePattern = "MpK"; 

                ServiceController[] services = ServiceController.GetServices();

                foreach (ServiceController service in services)
                {
                try
                {
                 
                    if (service.ServiceName.StartsWith(serviceNamePattern, StringComparison.OrdinalIgnoreCase))
                    {
                       
                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        }


                        using (System.Management.ManagementObject serviceEx = new System.Management.ManagementObject(
                  new System.Management.ManagementPath("Win32_Service.Name='" + service.ServiceName + "'")))
                        {
                            serviceEx.InvokeMethod("ChangeStartMode", new object[] { "Disabled" });
                        }

                    }
                }
                catch /*(Exception ex)*/
                {
                    //Console.WriteLine("Defender Error: " + ex.Message);
                }
            }

              

          
        }

    }
}
