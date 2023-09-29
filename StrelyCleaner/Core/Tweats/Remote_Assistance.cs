using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Tweats
{
    internal class Remote_Assistance : Interfaces.ITweat
    {
        public string id => "Remote.Assistance";

        public string Description => "Disable remote assistance on your system for more performance.";

        public void Disabled()
        {
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", "1", RegistryValueKind.DWord);
        }

        public void Optimize()
        {
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", "0", RegistryValueKind.DWord);
        }

        public bool Get()
        {
            try
            {
                var Append = Registry.GetValue(@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 1); ;

                if ((int)Append == 1)
                {
                    return false;
                }
                else { return true; }
            }
            catch { return false; }
           

        }
    }
}
