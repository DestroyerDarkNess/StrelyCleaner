using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace StrelyCleaner.Core
{
    public class InstalledProgram
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string InstallLocation { get; set; }
        public string UninstallString { get; set; }
        public string Publisher { get; set; }
        public string InstallDate { get; set; }
        public string EstimatedSize { get; set; }
        public string Contact { get; set; }
        public string HelpLink { get; set; }
        public string URLInfoAbout { get; set; }
        public string Comments { get; set; }
    }

    public class InstalledProgramManager
    {
        public static List<InstalledProgram> GetInstalledPrograms()
        {
            List<InstalledProgram> installedPrograms = new List<InstalledProgram>();

            string[] registryKeys = new string[]
        {
            @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\Classes\Installer\Products"
        };

            foreach (string registryKey in registryKeys)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
                {
                    if (key != null)
                    {
                        foreach (string subKeyName in key.GetSubKeyNames())
                        {
                            using (RegistryKey subKey = key.OpenSubKey(subKeyName))
                            {
                                if (subKey != null)
                                {
                                    string name = subKey.GetValue("DisplayName") as string;
                                    string version = subKey.GetValue("DisplayVersion") as string;
                                    string installLocation = subKey.GetValue("InstallLocation") as string;
                                    string uninstallString = subKey.GetValue("UninstallString") as string;

                                    if (!string.IsNullOrEmpty(name))
                                    {
                                        bool exists = installedPrograms.Any(p =>
                                         p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                                         string.Equals(p.InstallLocation, installLocation, StringComparison.OrdinalIgnoreCase));

                                        if (!exists)
                                        {

                                            InstalledProgram program = new InstalledProgram
                                            {

                                                Name = name,
                                                Version = version,
                                                InstallLocation = installLocation,
                                                UninstallString = uninstallString,
                                                Publisher = subKey.GetValue("Publisher") as string,
                                                InstallDate = subKey.GetValue("InstallDate") as string,
                                                EstimatedSize = subKey.GetValue("EstimatedSize") as string,
                                                Contact = subKey.GetValue("Contact") as string,
                                                HelpLink = subKey.GetValue("HelpLink") as string,
                                                URLInfoAbout = subKey.GetValue("URLInfoAbout") as string,
                                                Comments = subKey.GetValue("Comments") as string

                                            };

                                            installedPrograms.Add(program);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return installedPrograms;
        }

        public static InstalledProgram GetProgramByName(string programName)
        {
            return GetInstalledPrograms().FirstOrDefault(p => p.Name.Equals(programName, StringComparison.OrdinalIgnoreCase));
        }

        public static void UninstallProgram(InstalledProgram program)
        {
            if (!string.IsNullOrEmpty(program.UninstallString))
            {
                System.Diagnostics.Process.Start(program.UninstallString);
            }
            else
            {
                throw new Exception("The uninstall command for this program could not be found.");
            }
        }

        public static void OpenProgramLocation(InstalledProgram program)
        {
            if (!string.IsNullOrEmpty(program.InstallLocation))
            {
                System.Diagnostics.Process.Start("explorer.exe", program.InstallLocation);
            }
            else
            {
                throw new Exception("The installation location for this program could not be found.");
            }
        }
    }
}
