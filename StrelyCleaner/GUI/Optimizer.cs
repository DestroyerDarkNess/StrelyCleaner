
using StrelyCleaner.Controls;
using StrelyCleaner.Core;
using StrelyCleaner.Core.Interfaces;
using StrelyCleaner.Core.Optimizer;
using StrelyCleaner.GUI.SubForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using XylonV2;
using XylonV2.ComputerInfo.Graph;

namespace StrelyCleaner.GUI
{
    public partial class Optimizer : Form, IRenderForm
    {
        private Random random = new Random();

        //public ProcessHacker.ProcessSystemProvider SystemProvider = new ProcessHacker.ProcessSystemProvider();

        //public XylonV2.ComputerInfo.WMI.CPU cpu = new XylonV2.ComputerInfo.WMI.CPU();
        //public XylonV2.ComputerInfo.WMI.RAM ram = new XylonV2.ComputerInfo.WMI.RAM();
        //public XylonV2.ComputerInfo.Graph.RAMGraph ramGraph = new XylonV2.ComputerInfo.Graph.RAMGraph();
        //public XylonV2.ComputerInfo.Graph.CPUGraph cpuGraph = new XylonV2.ComputerInfo.Graph.CPUGraph();

        //List<IGenOptions> IBoostOptions = null;

        IGenOptions RamCleaner = new RamCleaner();
        ServicesBoost ServicesCleaner = new ServicesBoost();

        public Optimizer()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            this.BackColor = Color.Transparent;
           
            guna2TrackBar1.Value = 0;
            guna2TrackBar1.Invalidate();
        }

        private void Optimizer_Load(object sender, EventArgs e)
        {
            LoadAllGames();
        }

        public void BeginFrame()
        {

            //guna2ProgressBar1.Value = ramGraph.Ram_Physical_Usage;
            //guna2ProgressBar2.Value = cpuGraph.CPU_Usage;

            if (Global_Instances.ProcessProvider != null)
            {
                if (Global_Instances.ProcessProvider.Enabled == false) { Global_Instances.ProcessProvider.Enabled = true; }

                guna2ProgressBar1.Value = (int)Global_Instances.RamUsage;
                guna2ProgressBar2.Value = (int)Global_Instances.CPUUsage;
            }


            label2.Text = "Average: " + Progress_X + "%";
            guna2ProgressBar3.Value = Progress_X;

          

            //    var Process_CPU = SystemProvider.Dictionary
            //  .Where(p => p.Value.CpuUsage > 0)  
            //  .OrderByDescending(p => p.Value.CpuUsage)  
            //  .ToList();

            //    var Process_RAM = SystemProvider.Dictionary
            //.Where(p => p.Value.WorkingSetHistory.LastOrDefault() > 0)
            //.OrderByDescending(p => p.Value.WorkingSetHistory.LastOrDefault())
            //.ToList();

            //foreach (var Proc in Process_CPU) {
            //    //Console.WriteLine(Proc.Value.Name + $" IsSystem {Proc.Value.FileName?.ToLower().Contains("windows")}  =  {Proc.Value.CpuUsage:N2} %");
            //}
        }

        bool Initialized = false;
        int Progress_X = 10;
        public void UpdateRenderData()
        {
            if (Initialized == false)
            {
                Initialized = true;

                //IBoostOptions = new List<IGenOptions> { new RamCleaner() };
            }

            //ramGraph.RefreshGraph(ram);
            //cpuGraph.RefreshGraph();

        

            if (Global_Instances.ProcessProvider != null)
            {
                if (Global_Instances.ProcessProvider.Enabled == false) { Global_Instances.ProcessProvider.Enabled = true; }

                int Ram  = (int)Global_Instances.RamUsage; //  ramGraph.Ram_Physical_Usage;
                int CPU = (int)Global_Instances.CPUUsage; //  cpuGraph.CPU_Usage;

                Progress_X = ((CPU + Ram) * 100) / 200;

                this.Invoke(new Action(() =>
                {
                    Point LastXPoint = chartControl2.Graph.LastOrDefault();
                    int YPoint = (Progress_X / 10);
                    int XPoint = LastXPoint.X + 1;

                    if (XPoint > 80) { chartControl2.Graph.Clear(); XPoint = 0; }

                    chartControl2.Graph.Add(new Point(XPoint, YPoint));

                    if (Progress_X < 10)
                    {
                        chartControl2.ForeColor = Color.Lime;
                    }
                    else if (Progress_X > 10 && Progress_X < 20)
                    {
                        chartControl2.ForeColor = Color.YellowGreen;
                    }
                    else if (Progress_X > 20 && Progress_X < 30)
                    {
                        chartControl2.ForeColor = Color.Yellow;
                    }
                    else if (Progress_X > 30 && Progress_X < 40)
                    {
                        chartControl2.ForeColor = Color.Orange;
                    }
                    else if (Progress_X > 40 && Progress_X < 50)
                    {
                        chartControl2.ForeColor = Color.OrangeRed;
                    }
                    else if (Progress_X > 50 && Progress_X < 60)
                    {
                        chartControl2.ForeColor = Color.DarkOrange;
                    }
                    else if (Progress_X > 60 && Progress_X < 70)
                    {
                        chartControl2.ForeColor = Color.DarkSalmon;
                    }
                    else if (Progress_X > 70 && Progress_X < 80)
                    {
                        chartControl2.ForeColor = Color.Tomato;
                    }
                    else if (Progress_X > 80 && Progress_X < 90)
                    {
                        chartControl2.ForeColor = Color.IndianRed;
                    }
                    else if (Progress_X > 90 && Progress_X < 100)
                    {
                        chartControl2.ForeColor = Color.Red;
                    }

                    //chartControl2.Invalidate();
                    PanelChart.Refresh();

                }));
            }



            //if (Ram < 30)
            //{
            //    guna2ProgressBar1.ProgressColor = Color.Lime; guna2ProgressBar1.ProgressColor2 = Color.Lime;
            //}
            //else if (Ram > 30 && Ram < 50)
            //{
            //    guna2ProgressBar1.ProgressColor = Color.LimeGreen; guna2ProgressBar1.ProgressColor2 = Color.LimeGreen;
            //}
            //else if (Ram > 50 && Ram < 60)
            //{
            //    guna2ProgressBar1.ProgressColor = Color.Yellow; guna2ProgressBar1.ProgressColor2 = Color.Yellow;
            //}
            //else if (Ram > 60 && Ram < 70)
            //{
            //    guna2ProgressBar1.ProgressColor = Color.Orange; guna2ProgressBar1.ProgressColor2 = Color.Orange;
            //}
            //else if (Ram > 70 && Ram < 100)
            //{
            //    guna2ProgressBar1.ProgressColor = Color.Red; guna2ProgressBar1.ProgressColor2 = Color.Red;
            //}

            //if (CPU < 30)
            //{
            //    guna2ProgressBar2.ProgressColor = Color.Lime; guna2ProgressBar2.ProgressColor2 = Color.Lime;
            //}
            //else if (CPU > 30 && CPU < 50)
            //{
            //    guna2ProgressBar2.ProgressColor = Color.LimeGreen; guna2ProgressBar2.ProgressColor2 = Color.LimeGreen;
            //}
            //else if (CPU > 50 && CPU < 60)
            //{
            //    guna2ProgressBar2.ProgressColor = Color.Yellow; guna2ProgressBar2.ProgressColor2 = Color.Yellow;
            //}
            //else if (CPU > 60 && CPU < 70)
            //{
            //    guna2ProgressBar2.ProgressColor = Color.Orange; guna2ProgressBar2.ProgressColor2 = Color.Orange;
            //}
            //else if (CPU > 70 && CPU < 100)
            //{
            //    guna2ProgressBar2.ProgressColor = Color.Red; guna2ProgressBar2.ProgressColor2 = Color.Red;
            //}

            //guna2ProgressBar3.ProgressColor = chartControl2.ForeColor; guna2ProgressBar3.ProgressColor2 = chartControl2.ForeColor;

            //SystemProvider.Run();
        }

        #region " Private Methods "


        #endregion

        #region " UI "

        private void guna2TrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
            DisableAll();
            switch (guna2TrackBar1.Value)
            {
                case 0:
                    label7.Text = "RAM and CPU only";
                    guna2CheckBox4.Checked = true;
                    break;
                case 1:
                    label7.Text = "Only the basics";
                    guna2CheckBox4.Checked = true;
                    guna2CheckBox6.Checked = true;
                    break;
                case 2:
                    label7.Text = "Normal";
                    guna2CheckBox4.Checked = true;
                    guna2CheckBox6.Checked = true;
                    guna2CheckBox1.Checked = true;
                    break;
                case 3:
                    label7.Text = "Aggressive";
                    guna2CheckBox4.Checked = true;
                    guna2CheckBox6.Checked = true;
                    guna2CheckBox1.Checked = true;
                    guna2CheckBox5.Checked = true;
                    break;
                case 4:
                    label7.Text = "Extreme";
                    guna2CheckBox4.Checked = true;
                    guna2CheckBox6.Checked = true;
                    guna2CheckBox1.Checked = true;
                    guna2CheckBox5.Checked = true;
                    guna2CheckBox3.Checked = true;
                    guna2CheckBox2.Checked = true;
                    break;
                default:
                    label7.Text = "Error";
                    break;
            }
        }

        private void DisableAll() {
            foreach (Control ConEx in guna2Panel1.Controls) {
                if (ConEx is Guna.UI2.WinForms.Guna2CheckBox) {
                    Guna.UI2.WinForms.Guna2CheckBox Check = (Guna.UI2.WinForms.Guna2CheckBox)ConEx;
                    Check.Checked = false;
                }
            }
        }

        #endregion

        bool Boost_Ex = false;
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                if (Boost_Ex == false) { Boost(); } else { Restore(); }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void Boost() {
            Boost_Ex = true;
            ServicesCleaner.Restore();


            int OldRam_STR = (int)Global_Instances.RamUsage; //  ramGraph.Ram_Physical_Usage;
            int OldCPU_STR = (int)Global_Instances.CPUUsage; //  cpuGraph.CPU_Usage;

            int OldVerage = Progress_X;

            this.Invoke(new Action(() =>
            {
                guna2ProgressBar4.Visible = true;
                guna2Button2.Enabled = false;
                guna2Button1.Enabled = false;
                guna2Button1.Text = "Boosting...";
                guna2Button3.Visible = guna2CheckBox5.Checked;

            }));

            ServicesCleaner.Restore();

            if (guna2CheckBox1.Checked == true)
            {
                guna2ProgressBar4.Maximum = 100;
                guna2ProgressBar4.Value = 0;

                switch (guna2TrackBar1.Value)
                {
                    case 2:
                        PowerPlanManager.SetPowerPlan(PowerPlanManager.PowerPlan.PowerSaver);
                        break;
                    case 3:
                        PowerPlanManager.SetPowerPlan(PowerPlanManager.PowerPlan.Balanced);
                        break;
                    case 4:
                        PowerPlanManager.SetPowerPlan(PowerPlanManager.PowerPlan.HighPerformance);
                        break;
                    default:
                        break;
                }

                for (int i = 1; i <= guna2ProgressBar4.Maximum; i++)
                {
                    guna2ProgressBar4.Value += 1;
                    Utilities.Sleep(10, Utilities.Measure.Milliseconds);
                }
                //Utilities.Sleep(1);
            }

            if (guna2CheckBox4.Checked == true) {

                guna2ProgressBar4.Maximum = 6;
                for (int i = 0; i <= 5; i++)
                {
                    RamCleaner.Execute();
                    guna2ProgressBar4.Value += 1;
                    Utilities.Sleep(1);
                }

            }

            if (guna2CheckBox5.Checked == true)
            {
                guna2ProgressBar4.Maximum = 100;
                guna2ProgressBar4.Value = 0;

                switch (guna2TrackBar1.Value)
                {
                    case 2:
                        ServicesCleaner.Boost(ServicesBoost.ServiceMode.Normal);
                        break;
                    case 3:
                        ServicesCleaner.Boost(ServicesBoost.ServiceMode.Aggressive);
                        break;
                    case 4:
                        ServicesCleaner.Boost(ServicesBoost.ServiceMode.Extreme);
                        break;
                    default:
                        break;
                }

                for (int i = 1; i <= guna2ProgressBar4.Maximum; i++)
                {
                    guna2ProgressBar4.Value += 1;
                    Utilities.Sleep(10, Utilities.Measure.Milliseconds);
                }
                //Utilities.Sleep(1);
            }

            if (guna2CheckBox6.Checked == true)
            {
              
                List<string> Files = FileDirSearcher.GetFilePaths(SystemPaths.Temp, SearchOption.AllDirectories).ToList();

                guna2ProgressBar4.Maximum = Files.Count;

                foreach (string FileEx in Files)
                {
                    try { System.IO.File.Delete(FileEx); } catch { }
                    guna2ProgressBar4.Value += 1;
                    Utilities.Sleep(50, Utilities.Measure.Milliseconds);
                }

                List<string> Folders = FileDirSearcher.GetDirPaths(SystemPaths.Temp, SearchOption.AllDirectories).ToList();

                guna2ProgressBar4.Maximum = Folders.Count;

                foreach (string Dir in Folders)
                {
                    try { if (System.IO.Directory.Exists(Dir) == true) System.IO.Directory.Delete(Dir, true); } catch { }
                    guna2ProgressBar4.Value += 1;
                    Utilities.Sleep(50, Utilities.Measure.Milliseconds);
                }

            }

            int NewRam_STR = (int)Global_Instances.RamUsage; //  ramGraph.Ram_Physical_Usage;
            int NewCPU_STR = (int)Global_Instances.CPUUsage; //  cpuGraph.CPU_Usage;

            int NewVerage = Progress_X;


            this.Invoke(new Action(() =>
            {
                guna2ProgressBar4.Visible = false;
                guna2Button2.Enabled = true;
                guna2Button1.Enabled = true;
                guna2Button1.Text = "Boost";
                Boost_Ex = false;

                string RamEx = string.Empty; string CpuEx = string.Empty; string AverageEx = string.Empty;

                if (NewRam_STR < OldRam_STR) { RamEx = "Released RAM: " + (OldRam_STR - NewRam_STR) + "%"; }

                if (NewCPU_STR < OldCPU_STR) { CpuEx = "Released CPU: " + (OldCPU_STR - NewCPU_STR) + "%"; }

                if (NewVerage < OldVerage) { AverageEx = "old Avg: " + OldVerage + "%  New AVG: " + NewVerage + "%"; }

                string Message = string.Empty;

               if (!string.IsNullOrEmpty(RamEx)) { Message += RamEx + "   |   ";   }

               if (!string.IsNullOrEmpty(CpuEx)) {  Message += CpuEx + "   |   "; }

               if (!string.IsNullOrEmpty(AverageEx)) {  Message += AverageEx + "   |   "; }

                try { if (!string.IsNullOrEmpty(Message)) { if (this.Visible == true) Global_Instances.MainUI.ShowMessage(Message); } } catch { }

            }));
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Restore();
        }

        private void Restore()
        {
            this.Invoke(new Action(() =>
            {
                guna2Button3.Visible = false;
                ServicesCleaner.Restore();
            }));
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string GameFile = openFileDialog1.FileName;
            if (System.IO.File.Exists(GameFile)) {

                List<string> GameList = Global_Instances.AppSettings.ReadIniList("Booster", "Games");
                GameList.Add(GameFile);

                Global_Instances.AppSettings.WriteIniList("Booster", "Games", GameList);

                LoadAllGames();
            }
        }

        ControlLister Listener_Games = new ControlLister { OrientationControls = Orientation.Horizontal, Margen = new Point(5, 5) };
        private void LoadAllGames() {
            RemoveAll();
            panel2.Parent = this;
            List<string> GameList = Global_Instances.AppSettings.ReadIniList("Booster", "Games");

            Listener_Games.Add(panelFX1, panel2, false);

            foreach (string GameFile in GameList)
            {

                if (System.IO.File.Exists(GameFile)) {
                
                    GameShorcut GameItem = new GameShorcut();
                    GameItem.DataTag = GameFile;
                    GameItem.SetTooltip(GameFile);
                    GameItem.SetName(System.IO.Path.GetFileNameWithoutExtension(GameFile));
                    Image GameIcon = IconExtractor.ExtractIconFromFile(GameFile);
                    if (GameIcon != null) { GameItem.SetIcon(GameIcon);  }

                    GameItem.Click += (sender, e) =>
                    {
                        GameLauncher GameL = new GameLauncher(GameItem.DataTag);
                        GameL.Location = Global_Instances.MainUI.CenterForm(GameL);
                        GameL.Show();
                    };

                    Listener_Games.Add(panelFX1, GameItem, true);
                }

            }

        }

        private bool RemoveAll()
        {
            panel2.Parent = this;
            foreach (Control ControlEx in panelFX1.Controls)
            {
                //if ( string.Equals(ControlEx.Name, panel2.Name, StringComparison.OrdinalIgnoreCase) == false ) { 
                    panelFX1.Controls.Remove(ControlEx); 
                //}
            }
         return true;
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
