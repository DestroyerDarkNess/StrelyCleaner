using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Optimizer
{
    using System;
    using System.Management;

    public class PowerPlanManager
    {
        public enum PowerPlan
        {
            HighPerformance,
            Balanced,
            PowerSaver
        }

        public static bool SetPowerPlan(PowerPlan plan)
        {
            try
            {
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_PowerPlan");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection plans = searcher.Get();

                foreach (ManagementObject planObj in plans)
                {
                    if (plan == PowerPlan.HighPerformance && planObj["ElementName"].ToString() == "High performance")
                    {
                        planObj.InvokeMethod("Activate", null);
                        return true;
                    }
                    else if (plan == PowerPlan.Balanced && planObj["ElementName"].ToString() == "Balanced")
                    {
                        planObj.InvokeMethod("Activate", null);
                        return true;
                    }
                    else if (plan == PowerPlan.PowerSaver && planObj["ElementName"].ToString() == "Power saver")
                    {
                        planObj.InvokeMethod("Activate", null);
                        return true;
                    }
                }
            }
            catch //(Exception ex)
            {
                //Console.WriteLine("Error al cambiar el plan de energía: " + ex.Message);
            }

            return false;
        }

    }

}
