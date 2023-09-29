using Microsoft.VisualBasic;
using StrelyCleaner.Core.Optimizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrelyCleaner.Core
{
    public class Services
    {
        public  bool DefenderSvKiller = false;

        public int CurrentGameBoost = 0;

        Thread SvThread = null;
        public void Execute() {

            if (SvThread == null) {

                SvThread = new Thread(() =>
                {
                    while (true) {

                        SV_Booster();
                        SV_Tweat();

                        Utilities.Sleep(1, Utilities.Measure.Minutes);
                    }
                });

                SvThread.Start();

            }

        }

        private  void SV_Tweat () {

            if (DefenderSvKiller) { Core.Tweats.Defender.StopService();  }
        
        }

        private void SV_Booster()
        {
            try {

                Int64 phav = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
                Int64 tot = PerformanceInfo.GetTotalMemoryInMiB();
                decimal percentFree = ((decimal)phav / (decimal)tot) * 100;
                decimal percentOccupied = 100 - percentFree;
                if (Math.Round(percentOccupied) > Global_Instances.RAMPercent)
                {
                    Process[] process = Process.GetProcesses();
                    foreach (Process p in process) try { WinAPI.EmptyWorkingSet(p.Handle); } catch { }
                }

            } catch { }
           
        }


    }
}
