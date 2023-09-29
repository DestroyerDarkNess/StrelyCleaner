using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace StrelyCleaner.Core.Tweats
{
    internal class Timeouts : Interfaces.ITweat
    {
        public string id => "System.Timeouts";

        public string Description => "Optimizes the management of system times and tasks, improving efficiency and response.";

        public void Disabled()
        {
            Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true).DeleteValue("AutoEndTasks", false);
            Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true).DeleteValue("HungAppTimeout", false);
            Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true).DeleteValue("WaitToKillAppTimeout", false);
            Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true).DeleteValue("LowLevelHooksTimeout", false);
            Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "MenuShowDelay", "400");
            Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Mouse", "MouseHoverTime", "400");
        }

        public void Optimize()
        {
            Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "AutoEndTasks", "1");
            Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "HungAppTimeout", "1000");
            Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "MenuShowDelay", "0");
            Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "WaitToKillAppTimeout", "2000");
            Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LowLevelHooksTimeout", "1000");
            Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Mouse", "MouseHoverTime", "0");
        }

        public bool Get()
        {
            try
            {
                var A = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true).GetValue("AutoEndTasks", null);
                var B = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true).GetValue("HungAppTimeout", null);
                var C = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true).GetValue("WaitToKillAppTimeout", null);
                var D = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true).GetValue("LowLevelHooksTimeout", null);

                if (A == null || B == null || C == null || D == null)
                {
                    return false;
                }
                else { return true; }
            }
            catch { return false; }
           
        }
    }
}
