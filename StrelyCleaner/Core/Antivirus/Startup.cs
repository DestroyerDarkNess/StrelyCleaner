using StrelyCleaner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XylonV2.StartupManager.Services.Directories;
using XylonV2.StartupManager.Services.Registries;
using XylonV2.StartupManager.Models;
using System.Windows.Forms;
using Microsoft.Win32;

namespace StrelyCleaner.Core.Antivirus
{
    public class Startup : IScan
    {
        public string id => "Startup";

        public string Description => "Scan all Startup Apps";

        public List<ScanAction> Scan()
        {
            List<ScanAction> Result = new List<ScanAction>();
            try {

                string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";
                Utilities.CreateRegistryKeyIfNotExists(RegistryHive.CurrentUser, registryPath);

                Utilities.Sleep(1);

                RegistryService RegistryService = new RegistryService();
                DirectoryService DirectoryService = new DirectoryService();

                List<StartupList> startupPrograms = new List<StartupList>();

                var startupStates = RegistryService.GetStartupProgramStates();
                var registryStartups = RegistryService.GetStartupPrograms(startupStates);

                if (registryStartups != null)
                    startupPrograms.AddRange(registryStartups);

                var shellStartups = DirectoryService.GetStartupPrograms(startupStates);
                if (shellStartups != null)
                    startupPrograms.AddRange(shellStartups);

                foreach (StartupList ItemStartup in startupPrograms)
                {
                    string FileName = ItemStartup.GetParsedPath();

                    if (System.IO.File.Exists(FileName) == true && FileName != Application.ExecutablePath && System.IO.Path.GetFileName(FileName) != "desktop.ini")
                    {

                        ScanAction ScanObject = new ScanAction();
                        ScanObject.Object = FileName;
                        ScanObject.Type = ScanOptionType.File;
                        Result.Add(ScanObject);

                    }
                }

            } catch { }
            
            return Result;
        }
    }
}
