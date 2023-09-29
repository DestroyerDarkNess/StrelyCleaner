using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Tweats
{
    internal class Services : Interfaces.ITweat
    {
        public string id => "Services";

        public string Description => "Optimizes system services and adjusts settings to improve performance and efficiency.";

        public void Disabled()
        {
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control", "WaitToKillServiceTimeout", "5000");

            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", "Start", "2", RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service", "Start", "2", RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\dmwappushservice", "Start", "2", RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CldFlt", "Start", "2", RegistryValueKind.DWord);

            Utilities.StartService("DiagTrack");
            Utilities.StartService("diagnosticshub.standardcollector.service");
            Utilities.StartService("dmwappushservice");
            Utilities.StartService("CldFlt");

        }

        public void Optimize()
        {
            Utilities.StopService("DiagTrack");
            Utilities.StopService("diagsvc");
            Utilities.StopService("diagnosticshub.standardcollector.service");
            Utilities.StopService("dmwappushservice");
            Utilities.StopService("CldFlt");

            Utilities.RunCommand("sc config \"RemoteRegistry\" start= disabled");

            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DiagTrack", "Start", "4", RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagsvc", "Start", "4", RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\diagnosticshub.standardcollector.service", "Start", "4", RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\dmwappushservice", "Start", "4", RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\CldFlt", "Start", "4", RegistryValueKind.DWord);
        }

        public bool Get()
        {
            try
            {
                bool A = Utilities.ServiceIsRuning("DiagTrack");
                bool B = Utilities.ServiceIsRuning("diagsvc");
                bool C = Utilities.ServiceIsRuning("diagnosticshub.standardcollector.service");
                bool D = Utilities.ServiceIsRuning("dmwappushservice");
                bool E = Utilities.ServiceIsRuning("CldFlt");

                if (A == true || B == true || C == true || D == true)
                {
                    return false;
                }
                else { return true; }
            }
            catch { return false; }
          

        }
    }
}
