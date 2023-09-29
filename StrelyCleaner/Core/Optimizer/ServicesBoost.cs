using System;
using System.ServiceProcess;

namespace StrelyCleaner.Core.Optimizer
{
    public class ServicesBoost
    {
        private readonly string[] originalServices =
   {
        "AppMgmt", "WinRM", "DsmSvc", "p2pimsvc", "StiSvc", "SystemEventsBroker", "p2psvc",
        "lltdsvc", "NcaSvc", "wercplsupport", "PeerDistSvc", "SNMPTrap", "TrkWks", "WebClient",
        "svsvc", "TermService", "SCardSvr", "RemoteAccess", "QWAVE", "fhsvc", "W32Time", "SSDPSRV",
        "AxInstSV", "KtmRm", "CertPropSvc", "PNRPsvc", "swprv", "Wecsvc", "UmRdpService",
        "pla", "PcaSvc", "StorSvc", "DeviceAssociationService", "FontCache", "vmicshutdown",
        "WdNisSvc", "vmicheartbeat", "PNRPAutoReg", "ALG", "vmictimesync", "NetTcpPortSharing",
        "WMPNetworkSvc", "vmicrdv", "WerSvc", "EFS", "vmicvss", "WiaRpc", "RpcLocator", "VaultSvc"
    };

        public enum ServiceMode
        {
            Normal,
            Aggressive,
            Extreme
        }

        public void Boost(ServiceMode mode)
        {
            StopServices(mode);
        }

        public void Restore()
        {
            StartOriginalServices();
        }

        private void StopServices(ServiceMode mode)
        {
            string[] servicesToStop;

            switch (mode)
            {
                case ServiceMode.Normal:
                    servicesToStop = new string[]
                    {
                    "AppMgmt", "WinRM", "DsmSvc", "p2pimsvc", "StiSvc", "SystemEventsBroker", "p2psvc",
                    "lltdsvc", "NcaSvc", "wercplsupport", "PeerDistSvc", "SNMPTrap"
                    };
                    break;
                case ServiceMode.Aggressive:
                    servicesToStop = new string[]
                    {
                    "AppMgmt", "WinRM", "DsmSvc", "p2pimsvc", "StiSvc", "SystemEventsBroker", "p2psvc",
                    "lltdsvc", "NcaSvc", "wercplsupport", "PeerDistSvc", "SNMPTrap", "TrkWks", "WebClient",
                    "svsvc"
                    };
                    break;
                case ServiceMode.Extreme:
                    servicesToStop = originalServices;
                    break;
                default:
                   
                    return;
            }

            StopServices(servicesToStop);
        }

        private void StopServices(string[] serviceNames)
        {
            foreach (string serviceName in serviceNames)
            {
                try
                {
                    ServiceController serviceController = new ServiceController(serviceName);
                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        serviceController.Stop();
                        //serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                        //Console.WriteLine($"Detenido servicio: {serviceName}");
                    }
                    else
                    {
                        //Console.WriteLine($"El servicio {serviceName} no estaba en ejecución.");
                    }
                }
                catch //(Exception ex)
                {
                    //Console.WriteLine($"Error al detener el servicio {serviceName}: {ex.Message}");
                }
            }
        }

        private void StartOriginalServices()
        {
            foreach (string serviceName in originalServices)
            {
                try
                {
                    ServiceController serviceController = new ServiceController(serviceName);
                    if (serviceController.Status != ServiceControllerStatus.Running)
                    {
                        serviceController.Start();
                        //serviceController.WaitForStatus(ServiceControllerStatus.Running);
                        //Console.WriteLine($"Iniciado servicio: {serviceName}");
                    }
                    else
                    {
                        //Console.WriteLine($"El servicio {serviceName} ya estaba en ejecución.");
                    }
                }
                catch //(Exception ex)
                {
                    //Console.WriteLine($"Error al iniciar el servicio {serviceName}: {ex.Message}");
                }
            }
        }
    }

}
