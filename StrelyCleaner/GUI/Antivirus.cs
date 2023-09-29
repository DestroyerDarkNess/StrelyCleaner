using Guna.Charts.WinForms;
using StrelyCleaner.Core.Antivirus;
using StrelyCleaner.Core;
using StrelyCleaner.Core.Interfaces;
using StrelyCleaner.GUI.SubForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XylonV2.Core.Engine.Watcher;
using XylonV2.Engine.External.WindowsDefender;
using XylonV2.StartupManager.Models;
using StrelyCleaner.Controls;
using StrelyCleaner.Helpers;
using Guna.UI2.WinForms;
using System.Runtime.InteropServices.ComTypes;

namespace StrelyCleaner.GUI
{
    public partial class Antivirus : Form, IRenderForm
    {

        DriveWatcher DriveMon = new DriveWatcher();
        private ScrollManager panelFX2Scroll = null;
        public Antivirus()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            this.BackColor = Color.Transparent;
            DriveMon.DriveStatusChanged += DriveMon_DriveStatusChanged1;
            DriveMon.Start();
            gunaChart1.Dock = DockStyle.Fill;
            gunaChart1.ApplyConfig(Config(), Color.FromArgb(38, 41, 59));
            Example(gunaChart1);
            panelFX2Scroll = new ScrollManager(panelFX1, new Control[] { guna2VScrollBar1 }, true);
        }

        private void Antivirus_Load(object sender, EventArgs e)
        {
            ListDrivers();
        }

        #region " USB "

        public void ListDrivers() {
            panelFX1.Controls.Clear();
            ControlLister Listener_Drives = new ControlLister { OrientationControls = Orientation.Vertical, Margen = new Point(16, 10) };
            foreach (var Drive in DriveMon.Drives)
            {
                if (Drive.DriveType == System.IO.DriveType.Removable)
                {

                    try {

                        DeviceControl deviceControl = new DeviceControl();
                        deviceControl.OnAction = () => OpenScanner(ScanType.USB, Drive.RootDirectory.FullName, deviceControl.FixUSB, deviceControl.FixWithoutScan);
                        deviceControl.SetName(Drive.Name);
                        deviceControl.SetSize(string.Format("{0} GB", (Drive.TotalSize / (Math.Pow(1024, 3))).ToString("n1")));
                        deviceControl.SetTooltip(Drive.VolumeLabel);
                        deviceControl.MainDir = Drive.RootDirectory.FullName;
                        deviceControl.ScanDrive();
                        Listener_Drives.Add(panelFX1, deviceControl);
                        double Porcent = Math.Round(((Drive.AvailableFreeSpace / (Math.Pow(1024, 3))) * 100) / (Drive.TotalSize / (Math.Pow(1024, 3))));
                        deviceControl.SetPorcent(Convert.ToInt32(Porcent));

                    } catch { }

                }
            }

        }


        #endregion

        private bool IsScaned = false;

        public void DetectThreads()
        {


         IScan StartupList = new Startup();
        List<AV_File> RiskFiles = new List<AV_File>();

            List<ScanAction> StartupActionList = StartupList.Scan();

            foreach (ScanAction action in StartupActionList)
            {

                try
                {

                    string FileName = action.Object;

                    if (System.IO.File.Exists(FileName) == false) { continue; }

                    bool IsSystemFile = Utilities.IsSystem(FileName);

                    if (IsSystemFile == false && Utilities.IsScriptFormat(FileName) == true)
                    {
                        string GenMWName = XylonV2.CARO.VirNames.Generate(XylonV2.CARO.VirNames.Type.Virus,
                                XylonV2.CARO.VirNames.Platforms.Win32, XylonV2.CARO.VirNames.Family.Inde,
                                XylonV2.CARO.VirNames.VariantL.D, XylonV2.CARO.VirNames.Suffixes.gen).ToString();
                        RiskFiles.Add(new AV_File(FileName, GenMWName)); continue;
                    }

                    if (IsSystemFile == false)
                    {
                        if (System.IO.File.Exists(SystemPaths.DefenderExeLocation) == true)
                        {


                            WindowsDefenderScanner scanner = new WindowsDefenderScanner(SystemPaths.DefenderExeLocation);
                            XylonV2.Engine.External.Core.ScanResult result = scanner.Scan(FileName);


                            if (result == XylonV2.Engine.External.Core.ScanResult.ThreatFound)
                            {
                                RiskFiles.Add(new AV_File(FileName, scanner.ResultParsed)); continue;
                            }


                        }
                    }

                    if (IsSystemFile == false && XylonV2.Engine.PE.Binary.PEChecker.IsNetAssembly(FileName) == true)
                    {
                        XylonV2.Engine.External.Core.DetectionResult ScanResult = XylonV2.Engine.PE.Net.Core.NetAnalysis.NetScanner(FileName);

                        if (ScanResult.Result == XylonV2.Engine.External.Core.ScanResult.ThreatFound)
                        {
                            RiskFiles.Add(new AV_File(FileName, ScanResult.Signature)); continue;
                        }
                    }

                    if (IsSystemFile == false)
                    {
                        XylonV2.Engine.External.Core.DetectionResult ScanResult = XylonV2.Engine.PE.Analysis.StringScan(FileName);

                        if (ScanResult.Result == XylonV2.Engine.External.Core.ScanResult.ThreatFound)
                        {
                            RiskFiles.Add(new AV_File(FileName, ScanResult.Signature)); continue;
                        }
                    }


                }
                catch { }

            }

          

            this.Invoke(new Action(() =>
            {
                if (RiskFiles.Count == 0) {
                    guna2GradientButton1.Text = "You Are Protected     ";
                    guna2GradientButton1.Checked = true;
                    guna2HtmlToolTip1.SetToolTip(guna2GradientButton1, "Great!!, Your system is clean.");
                
                } else {
                    guna2GradientButton1.Text = "Possible threats found!!     ";
                    guna2GradientButton1.Checked = false;
                    guna2HtmlToolTip1.SetToolTip(guna2GradientButton1, "Threats were found, please perform a \"Smart Scan\", it will only take a few seconds.");
                }
               
            }));



        }

        public void BeginFrame()
        {
            if (Global_Instances.ProcessProvider != null)
            {
                if (Global_Instances.ProcessProvider.Enabled == true) { Global_Instances.ProcessProvider.Enabled = false; }
            }

        }

        public void UpdateRenderData()
        {

            if (IsScaned == false) {
                IsScaned = true;
                DetectThreads();
            }

        }

        public void OpenScanner(ScanType Scan , string Path = "", bool TryFix = false, bool NoScan = false) {
            
            AVScanner CurrentScanner = new AVScanner(Scan)  { TopLevel = false, Text = "AVScanner", Visible = false, BackColor = Color.Transparent };
            CurrentScanner.CustomScanPath = Path;
            CurrentScanner.FixDevice = TryFix;
            CurrentScanner.NoAnalize = NoScan;
            this.Controls.Add(CurrentScanner);
            CurrentScanner.Visible = true;
            CurrentScanner.Dock = DockStyle.Fill;
            CurrentScanner.BringToFront();
            panel3.Visible = false;
        }

        public void ShowMessage(string msg  ) {
         if (this.Visible == true)  Global_Instances.MainUI.ShowMessage(msg);
        }

        public void CloseScanner(Form ToClose, Control Container)
        {
            if (ToClose != null) {
                Container.Controls.Remove(ToClose);
                ToClose.Dispose();
            }
            panel3.Visible = true;
            panel3.BringToFront();
            if (IsScaned == true) { IsScaned = false; }
        }

        #region " DriveWatcher "

        private void DriveMon_DriveStatusChanged1(object sender, DriveWatcher.DriveStatusChangedEventArgs e)
        {
            //switch (e.DeviceEvent)
            //{
            //    case DriveWatcher.DeviceEvents.Arrival:
            //        {
            //            //StringBuilder sb = new StringBuilder();
            //            //sb.AppendLine("New drive connected...'");
            //            //sb.AppendLine(string.Format("Type......: {0}", e.DriveInfo.DriveType.ToString()));
            //            //sb.AppendLine(string.Format("Label.....: {0}", e.DriveInfo.VolumeLabel));
            //            //sb.AppendLine(string.Format("Name......: {0}", e.DriveInfo.Name));
            //            //sb.AppendLine(string.Format("Root......: {0}", e.DriveInfo.RootDirectory));
            //            //sb.AppendLine(string.Format("FileSystem: {0}", e.DriveInfo.DriveFormat));
            //            //sb.AppendLine(string.Format("Size......: {0} GB", (e.DriveInfo.TotalSize / (Math.Pow(1024, 3))).ToString("n1")));
            //            //sb.AppendLine(string.Format("Free space: {0} GB", (e.DriveInfo.AvailableFreeSpace / (Math.Pow(1024, 3))).ToString("n1")));
            //            //double Porcent = Math.Round(((e.DriveInfo.AvailableFreeSpace / (Math.Pow(1024, 3))) * 100) / (e.DriveInfo.TotalSize / (Math.Pow(1024, 3))));
            //            //sb.AppendLine(string.Format("Porcent: {0} %", Porcent.ToString()));
            //            //Console.WriteLine(sb.ToString());
            //            break;
            //        }

            //    case DriveWatcher.DeviceEvents.RemoveComplete:
            //        {
            //            //StringBuilder sb = new StringBuilder();
            //            //sb.AppendLine("Drive disconnected...'");
            //            //sb.AppendLine(string.Format("Name: {0}", e.DriveInfo.Name));
            //            //sb.AppendLine(string.Format("Root: {0}", e.DriveInfo.RootDirectory));
            //            //Console.WriteLine(sb.ToString());
            //            break;
            //        }
            //}
            ListDrivers();
        }

        #endregion

        #region " Chart "


        public static void Example(Guna.Charts.WinForms.GunaChart chart)
        {
            string[] months = { "January", "February", "March", "April", "May", "June", "July",
                "August", "September", "October", "November", "December"  };

            //Chart configuration 
            chart.YAxes.GridLines.Display = false;

            //Create a new dataset 
            var dataset = new Guna.Charts.WinForms.GunaSplineDataset();
            dataset.PointRadius = 3;
            dataset.PointStyle = PointStyle.Circle;
            var r = new Random();
            for (int i = 0; i < months.Length; i++)
            {
                //random number
                int num = r.Next(10, 50);

                dataset.DataPoints.Add(months[i], num);
            }

            //Add a new dataset to a chart.Datasets
            chart.Datasets.Add(dataset);

            //An update was made to re-render the chart
            chart.Update();
        }

        public ChartConfig Config()
        {
            ChartConfig config = new ChartConfig();
            Color gridColor = Color.FromArgb(49, 52, 82);
            Color foreColor = Color.FromArgb(177, 182, 205);
            Color[] colors = new Color[] { Color.FromArgb(140, 81, 165), Color.FromArgb(203, 94, 152), Color.FromArgb(244, 123, 138), Color.FromArgb(255, 163, 127), Color.FromArgb(255, 210, 133) };

            var chartFont = new Guna.Charts.WinForms.ChartFont()
            {
                FontName = "Segoe UI",
                Size = 10,
                Style = Guna.Charts.WinForms.ChartFontStyle.Normal
            };

            config.Title.ForeColor = foreColor;

            config.Legend.LabelFont = chartFont;
            config.Legend.LabelForeColor = foreColor;

            config.XAxes.GridLines.Color = gridColor;
            config.XAxes.GridLines.ZeroLineColor = gridColor;
            config.XAxes.Ticks.Font = chartFont;
            config.XAxes.Ticks.ForeColor = foreColor;

            config.YAxes.GridLines.Color = gridColor;
            config.YAxes.GridLines.ZeroLineColor = gridColor;
            config.YAxes.Ticks.Font = chartFont;
            config.YAxes.Ticks.ForeColor = foreColor;

            config.ZAxes.GridLines.Color = gridColor;
            config.ZAxes.GridLines.ZeroLineColor = gridColor;
            config.ZAxes.Ticks.Font = chartFont;
            config.ZAxes.Ticks.ForeColor = foreColor;
            config.ZAxes.PointLabels.Font = chartFont;
            config.ZAxes.PointLabels.ForeColor = foreColor;

            config.PaletteCustomColors.FillColors.AddRange(colors);
            config.PaletteCustomColors.BorderColors.AddRange(colors);
            config.PaletteCustomColors.PointFillColors.AddRange(colors);
            config.PaletteCustomColors.PointBorderColors.AddRange(colors);

            return config;
        }


        #endregion

        #region " Scan Selection "

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            OpenScanner(ScanType.Full);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            OpenScanner(ScanType.Smart);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Helpers.FileBorserDialog.FolderBrowserDialog FolderDialog = new Helpers.FileBorserDialog.FolderBrowserDialog();

            if (FolderDialog.ShowDialog(this) == DialogResult.OK)
            {
                OpenScanner(ScanType.Custom, FolderDialog.DirectoryPath);
            }

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            if (guna2GradientButton1.Checked == false) { OpenScanner(ScanType.Smart); }
        }

        #endregion


    }
}
