using StrelyCleaner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Optimizer
{
    internal class RamCleaner : IGenOptions
    {
        public string id => "Ram_Reducer";

        public string Description => "";

        public void Execute() {   foreach (Process p in Process.GetProcesses()) try { WinAPI.EmptyWorkingSet(p.Handle); } catch { } }

    }
}
