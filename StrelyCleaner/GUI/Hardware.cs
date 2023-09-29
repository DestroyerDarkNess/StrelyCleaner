
using Guna.UI2.WinForms;
using HydraHelper.Adders.Base;
using OpenHardwareMonitor.Hardware;
using ProcessHacker.Native.Threading;
using StrelyCleaner.Controls;
using StrelyCleaner.Core;
using StrelyCleaner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using XylonV2.ComputerInfo.Graph;

namespace StrelyCleaner.GUI
{
    public partial class Hardware : Form , IRenderForm
    {
        public OpenHardwareMonitor.Hardware.Computer computer = null;

        //public XylonV2.ComputerInfo.WMI.CPU  cpu = null;
        //public XylonV2.ComputerInfo.WMI.RAM  ram = null;
        //public XylonV2.ComputerInfo.Graph.RAMGraph ramGraph = null;
        //public XylonV2.ComputerInfo.Graph.CPUGraph  cpuGraph = null;

        List<KeyValuePair<string, string>> OsInfo = null;
        List<KeyValuePair<string, string>> BiosInfo = null;
        List<KeyValuePair<string, string>> GraphicsInfo = null;
        List<KeyValuePair<string, string>> CPUInfo = null;
        List<KeyValuePair<string, string>> BoardInfo = null;
        List<Core.Hardware.SYSInfoMonitorLib.RAM> RamInfo = null;

        private bool IsStaticInfoLoaded = false;
        //private bool FixCounters = false;
        private string CPU_Temp = string.Empty;

        private ScrollManager panelFX2Scroll = null;

        public Hardware()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
           
            InitializeComponent();
            this.BackColor = Color.Transparent;
            
            panelFX2Scroll = new ScrollManager(panelFX2, new Control[] { guna2VScrollBar1 }, true);
        }

        public void BeginFrame()
        {
            
            if (IsStaticInfoLoaded == false) {
                IsStaticInfoLoaded = true;
                try {

                   

                    label3.Text = Core.HardwareInfo.Info.GetComputerManufacturer() + " " + Core.HardwareInfo.Info.GetComputerModel();

                    //cpu = new XylonV2.ComputerInfo.WMI.CPU();
                    //cpuGraph = new XylonV2.ComputerInfo.Graph.CPUGraph();
                    label5.Text = Environment.ProcessorCount + " Cores";

                    //ram = new XylonV2.ComputerInfo.WMI.RAM();
                    //ramGraph = new XylonV2.ComputerInfo.Graph.RAMGraph();
                    //double DoubleBytes = System.Convert.ToDouble(ram.PysicalSize / (double)1073741824);
                    label4.Text = Helpers.Functions.Round_Bytes( Core.Optimizer.PerformanceInfo.GetTotalMemoryInMiB());

                }
                catch (Exception ex) { ExceptionManager.WriteConsoleError(ex); }

                try {
                    computer = new OpenHardwareMonitor.Hardware.Computer() { CPUEnabled = true, MainboardEnabled = true };
                    computer.Open();
                }
                catch { computer = null; }


            }
           

            //if (guna2Panel1.Visible == false) { guna2Panel1.Visible = true; }
            //guna2Panel1.Invalidate();

            //if (ram != null && ram.Update() == true) { guna2ProgressBar1.Value = ramGraph.Ram_Physical_Usage; }

            //if (cpuGraph != null && cpuGraph.CPU_Usage > 1)   { guna2ProgressBar2.Value = cpuGraph.CPU_Usage;  }

            if (Global_Instances.ProcessProvider != null) {
                if (Global_Instances.ProcessProvider.Enabled == false) { Global_Instances.ProcessProvider.Enabled = true; }
            
                guna2ProgressBar1.Value = (int)Global_Instances.RamUsage;
                guna2ProgressBar2.Value = (int)Global_Instances.CPUUsage; 
            }

            if (String.IsNullOrEmpty(CPU_Temp) == false) { label5.Text = CPU_Temp + " °C"; }


            //if (cpuGraph != null && cpuGraph.ErrorOnCounter == true && FixCounters == false)
            //{
            //    FixCounters = true;
            //}

            guna2ProgressBar1.Invalidate();
            guna2ProgressBar2.Invalidate();

            if (panelFX1.Visible == false && IsInfoLoaded == false) { if (OsInfo != null)  { IsInfoLoaded = true; panelFX1.Visible = true; guna2Button1.Checked = true;  } }
        }

        private bool IsInfoLoaded = false;

        public void UpdateRenderData()
        {
            UpdateInfo();
        }

        //private bool FixExecute = false;

      

        public  bool UpdateInfo() {
          
            if (OsInfo == null) { try { OsInfo = Core.Hardware.SYSInfoMonitorLib.GetOSInfo(); } catch (Exception Ex) { ExceptionManager.WriteConsoleError(Ex, "SYSInfoMonitorLib.GetOSInfo"); } }

            if (CPUInfo == null) { try { CPUInfo = Core.Hardware.SYSInfoMonitorLib.GetProcessorInfoInKeyValuePair(); } catch (Exception Ex) { ExceptionManager.WriteConsoleError(Ex, "SYSInfoMonitorLib.GetProcessorInfoInKeyValuePair"); } }

            if (RamInfo == null) { try { RamInfo = Core.Hardware.SYSInfoMonitorLib.GetRAM(); } catch (Exception Ex) { ExceptionManager.WriteConsoleError(Ex, "SYSInfoMonitorLib.GetRAM"); } }

            if (GraphicsInfo == null) { try { GraphicsInfo = Core.Hardware.SYSInfoMonitorLib.GetGraphicsInfo(); } catch (Exception Ex) { ExceptionManager.WriteConsoleError(Ex, "SYSInfoMonitorLib.GetGraphicsInfo"); } }

            if (BiosInfo == null) { try { BiosInfo = Core.Hardware.SYSInfoMonitorLib.GetBIOS(); } catch (Exception Ex) { ExceptionManager.WriteConsoleError(Ex, "SYSInfoMonitorLib.GetBIOS"); } }

            if (BoardInfo == null) { try { BoardInfo = Core.Hardware.SYSInfoMonitorLib.GetBaseBoard(); } catch (Exception Ex) { ExceptionManager.WriteConsoleError(Ex, "SYSInfoMonitorLib.GetBaseBoard"); } }


            try
            {

                if (computer != null)
                {

                    foreach (var hardwareItem in computer.Hardware)
                    {
                        if (hardwareItem.HardwareType == HardwareType.CPU)
                        {
                            hardwareItem.Update();

                            var cpuTemperature = hardwareItem.Sensors
                                .Where(sensor => sensor.SensorType == SensorType.Temperature)
                                .Select(sensor => sensor.Value)
                                .LastOrDefault();

                            if (cpuTemperature != null) { CPU_Temp = cpuTemperature.ToString(); }

                            break;
                        }
                    }

                }

                //ramGraph?.RefreshGraph(ram);
                //cpuGraph?.RefreshGraph();

                //if (FixCounters == true && FixExecute == false)
                //{
                //    FixExecute = true;
                //    Core.Utilities.RunCommand("cd C:\\Windows\\system32 & lodctr /r");
                //    Core.Utilities.RunCommand("cd C:\\Windows\\SysWOW64 & lodctr /r");
                //}

                return true;
            }
            catch (Exception Ex)  { ExceptionManager.WriteLogError(Ex);  return false; }
        }

        #region " Sys Info "
        private void guna2Button1_CheckedChanged(object sender, EventArgs e)
        {
            if (OsInfo == null || guna2Button1.Checked == false) { return; }
            panelFX2.Controls.Clear(); CircleProgress(true);
            panel2.BackgroundImage = guna2Button1.Image;
            label6.Text = OsInfo[0].Value.ToString();
            LoadOsInfo(panelFX2, OsInfo);
        }

        private void guna2Button2_CheckedChanged(object sender, EventArgs e)
        {
            if (CPUInfo == null || guna2Button2.Checked == false) { return; }
            panelFX2.Controls.Clear(); CircleProgress(true);
            panel2.BackgroundImage = guna2Button2.Image;
            label6.Text = CPUInfo[0].Value.ToString();
            LoadOsInfo(panelFX2, CPUInfo);
        }

        private void guna2Button3_CheckedChanged(object sender, EventArgs e)
        {
            if (GraphicsInfo == null || guna2Button3.Checked == false) { return; }
            panelFX2.Controls.Clear(); CircleProgress(true);
            panel2.BackgroundImage = guna2Button3.Image;
            label6.Text = GraphicsInfo[0].Value.ToString();
            LoadOsInfo(panelFX2, GraphicsInfo);
        }

        private void guna2Button4_CheckedChanged(object sender, EventArgs e)
        {
            if (BiosInfo == null || guna2Button4.Checked == false) { return;  }
            panelFX2.Controls.Clear(); CircleProgress(true);
            panel2.BackgroundImage = guna2Button4.Image;
            label6.Text = BiosInfo[1].Value.ToString() + " " + BiosInfo[0].Value.ToString();
            LoadOsInfo(panelFX2, BiosInfo);
        }

        private void guna2Button5_CheckedChanged(object sender, EventArgs e)
        {
            if (BoardInfo == null || guna2Button5.Checked == false) { return; }
            panelFX2.Controls.Clear(); CircleProgress(true);
            panel2.BackgroundImage = guna2Button5.Image;
            label6.Text =   BoardInfo[2].Value.ToString() + " " + BoardInfo[5].Value.ToString();;
            LoadOsInfo(panelFX2, BoardInfo);
        }

        private void guna2Button6_CheckedChanged(object sender, EventArgs e)
        {
            if (RamInfo == null || guna2Button6.Checked == false) { return; }  
            panelFX2.Controls.Clear(); CircleProgress(true);
            panel2.BackgroundImage = guna2Button6.Image;
            label6.Text = RamInfo.Count + " Slots, Total: " + label4.Text;
            LoadRamInfo(panelFX2, RamInfo);
        }

        private void CircleProgress(bool Show) {

            if (Show == true)
            {
                guna2CircleProgressBar1.Animated = true;
                guna2CircleProgressBar1.Visible = true;
                guna2CircleProgressBar1.Invalidate();
            } else
            {
                guna2CircleProgressBar1.Animated = false;
                guna2CircleProgressBar1.Visible = false;
            }
        
        }

        private  void LoadOsInfo(Panel PanelContainer, List<KeyValuePair<string, string>>  Data) {

            Utilities.Sleep(1);

            ControlLister Listener_Sys = new ControlLister { OrientationControls = Orientation.Horizontal, Margen = new Point(10, 10) };
            foreach (var ItemInfo in Data)
            {
                if (String.IsNullOrWhiteSpace(ItemInfo.Value) == true) { continue; }

                if (ItemInfo.Key == "InstalledDisplayDrivers")
                {
                    Listener_Sys.OrientationControls = Orientation.Vertical;
                    Control LabelInfo = CreateLabel("", Color.White, Color.DodgerBlue, ItemInfo.Key + " -> ");
                    Listener_Sys.Add(PanelContainer, LabelInfo, false);

                    string[] Drivers = ItemInfo.Value.Split(',');
                   

                    foreach (string DriverInfo in Drivers) {
                        Control LabelDriver = CreateLabel(DriverInfo, Color.White, Color.DodgerBlue);
                        Console.WriteLine(DriverInfo);
                        Listener_Sys.Add(PanelContainer, LabelDriver, false);
                        //Thread.Sleep(100);
                    }


                } else {
                  
                    Control LabelInfo = CreateLabel(ItemInfo.Value, Color.White, Color.DodgerBlue, ItemInfo.Key + " -> ");
                    Listener_Sys.Add(PanelContainer, LabelInfo, true);
                    if (Listener_Sys.OrientationControls == Orientation.Vertical) { Listener_Sys.OrientationControls = Orientation.Horizontal; }

                }

            }


            panelFX2Scroll.UpdateScroll();
            CircleProgress(false);
        }


        private  void LoadRamInfo(Panel PanelContainer, List<Core.Hardware.SYSInfoMonitorLib.RAM> Data)
        {

            Utilities.Sleep(1);

            ControlLister Listener_Sys = new ControlLister { OrientationControls = Orientation.Horizontal, Margen = new Point(10, 10) };
        


            foreach (var ItemInfo in Data)
            {

                Control EmptyInfo = CreateLabel("       ", Color.White, Color.DodgerBlue, "     ");

                Control BankInfo = CreateLabel(ItemInfo.BankLabel.ToString(), Color.White, Color.DodgerBlue, "Bank -> ");

                //BankInfo.Font. = 16.0f;

                Control CapacityInfo = CreateLabel( Math.Round(ItemInfo.Capacity.GigaBytes).ToString() + " GB", Color.White, Color.DodgerBlue, "Capacity -> ");

                Control ManufacturerInfo = CreateLabel(ItemInfo.Manufacturer.ToString(), Color.White, Color.DodgerBlue, "Manufacturer -> ");

                Control MemoryTypeInfo = CreateLabel(ItemInfo.MemoryType.ToString(), Color.White, Color.DodgerBlue, "MemoryType -> ");

                Control SpeedInfo = CreateLabel(ItemInfo.Speed.ToString(), Color.White, Color.DodgerBlue, "Speed -> ");

                Control FormFactorInfo = CreateLabel(ItemInfo.FormFactor.ToString(), Color.White, Color.DodgerBlue, "FormFactor -> ");

                Control[] ControlsArray = { BankInfo, CapacityInfo, ManufacturerInfo, MemoryTypeInfo, SpeedInfo, FormFactorInfo };

                foreach (var ItemText in ControlsArray)
                {
                   
                    Listener_Sys.Add(PanelContainer, ItemText, true);
                   
                }

                Listener_Sys.OrientationControls = Orientation.Vertical;

                Panel Separator = new Panel() { Width = 550, Height = 2};
                Separator.BackColor = Color.Silver;
                Listener_Sys.Add(PanelContainer, Separator, false);
                if (Listener_Sys.OrientationControls == Orientation.Vertical) { Listener_Sys.OrientationControls = Orientation.Horizontal; }
                Listener_Sys.Add(PanelContainer, EmptyInfo, true);
            }

            panelFX2Scroll.UpdateScroll();
            CircleProgress(false);
        }

        #endregion


        #region " Other Methods "
        private Control CreateLabel(string Text, Color ForeCol, Color InfoCol, string Info = "")
        {
            RichTextLabel ControlEx = new RichTextLabel();
            if (Info != "") { ControlEx.AppendText(Info, InfoCol, Color.Black); }
            ControlEx.AppendText(Text, ForeCol, Color.Black);
            ControlEx.AdjustRichTextBoxSize();
            return ControlEx;
        }





        #endregion

        private void Hardware_Load(object sender, EventArgs e)
        {
           
          
        }

        

    }
}
