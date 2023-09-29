using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Tweats
{
    internal class Explorer_AutoComplete : Interfaces.ITweat
    {
        public string id => "Explorer.AutoComplete";

        public string Description => "Disable Windows File Explorer AutoComplete to improve efficiency.";

        public void Disabled()
        {
            Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\AutoComplete", true).DeleteValue("Append Completion", false);
            Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\AutoComplete", true).DeleteValue("AutoSuggest", false);
        }

        public void Optimize()
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoComplete", "Append Completion", "yes", RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoComplete", "AutoSuggest", "yes", RegistryValueKind.String);
        }

        public bool Get()
        {
            try
            {
                string Append = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\AutoComplete", true).GetValue("Append Completion", string.Empty) as string;
                string AutoSuggest = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\AutoComplete", true).GetValue("AutoSuggest", string.Empty) as string;

                if (Append == string.Empty || AutoSuggest == string.Empty)
                {
                    return false;
                }
                else { return true; }
            }
            catch { return false; }
           

        }
    }
}
