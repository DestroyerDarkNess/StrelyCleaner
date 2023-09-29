using HydraHelper.Adders.Base;
using ProcessHacker;
using StrelyCleaner.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrelyCleaner.Core
{
    public class Global_Instances
    {

        public static string AppFolder = System.IO.Path.Combine(SystemPaths.Appdata_Local, @"Strely_Cleanner");
        public static string AppThemeFolder = System.IO.Path.Combine(AppFolder, @"Themes");
        public static string IniFile = System.IO.Path.Combine(AppFolder, "Config.ini");
        public static MainWindow MainUI = null;
        public static RenderUI RenderUI = null;
        public static SettingProvider AppSettings = null;

        public static ProcessSystemProvider ProcessProvider;
        public static int RAMPercent = 60;
        public static int timerinterval = 60;

        public static bool Lite = true;

        public static bool LoadSettings()
        {
            try {
                if (System.IO.Directory.Exists(AppFolder) == false) { System.IO.Directory.CreateDirectory(AppFolder); }

                if (System.IO.Directory.Exists(AppThemeFolder) == false) { System.IO.Directory.CreateDirectory(AppThemeFolder); }

                AppSettings = new SettingProvider(IniFile);
                bool LiteMode = Boolean.Parse(Global_Instances.AppSettings.ReadIni("Settings", "LiteMode", "True"));
                Global_Instances.Lite = LiteMode;

                return true;
            } catch { return false; }
        } 

        public static void CreateInstances() {
           
            string Img0 = System.IO.Path.Combine(AppThemeFolder, "0.jpg"); 
            if (File.Exists(Img0) == false) { Properties.Resources._0.Save(Img0, System.Drawing.Imaging.ImageFormat.Jpeg); }
            
            string Img1 = System.IO.Path.Combine(AppThemeFolder, "1.jpg");
            if (File.Exists(Img1) == false) { Properties.Resources._1.Save(Img1, System.Drawing.Imaging.ImageFormat.Jpeg); }
           
            string Img2 = System.IO.Path.Combine(AppThemeFolder, "2.jpg");
            if (File.Exists(Img2) == false) { Properties.Resources._2.Save(Img2, System.Drawing.Imaging.ImageFormat.Jpeg); }
            
            string Img3 = System.IO.Path.Combine(AppThemeFolder, "3.jpg");
            if (File.Exists(Img3) == false) { Properties.Resources._3.Save(Img3, System.Drawing.Imaging.ImageFormat.Jpeg); }
           
            Core.Settings.Experimental = Boolean.Parse(Global_Instances.AppSettings.ReadIni("Settings", "ExperimentalMode", "False"));
          
            ProcessProvider = new ProcessSystemProvider();
            ProcessProvider.Updated += UpdateStatusInfo;
            ProcessProvider.Enabled = true;
            //ProcessProvider.Run();

            ProviderThread PrimaryProviderThread = new ProviderThread(1000);
            PrimaryProviderThread.Add(ProcessProvider);


            MainUI = new MainWindow();


            if (Lite == false) {

                bool Services = Boolean.Parse(Global_Instances.AppSettings.ReadIni("Settings", "AppService", "False"));
                RenderUI = new RenderUI(Services);

            }  
        }

        public static int RamUsage = 0;
        public static int CPUUsage = 0;

        private static void UpdateStatusInfo()
        {
            if (Global_Instances.ProcessProvider != null)
            {
                float RamUsangeEx = ((float)(Global_Instances.ProcessProvider.System.NumberOfPhysicalPages - Global_Instances.ProcessProvider.Performance.AvailablePages) * 100 /
                    Global_Instances.ProcessProvider.System.NumberOfPhysicalPages);
                RamUsage = (int)RamUsangeEx;
            }

            if (Global_Instances.ProcessProvider != null)
            {
                float CPUUsageEx = (Global_Instances.ProcessProvider.CurrentCpuUsage * 100);
                CPUUsage = (int)CPUUsageEx;
            }
        }

    }
}
