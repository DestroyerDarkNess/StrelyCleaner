using Guna.UI2.WinForms;
using ProcessHacker.Common;
using StrelyCleaner.Core;
using StrelyCleaner.Core.Interfaces;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XylonV2.Core.Engine.Watcher;
using XylonV2.Core.Engine.WMI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StrelyCleaner.GUI
{
    public partial class MainWindow : Form
    {
        public Process CurrentProc = Process.GetCurrentProcess();
        public bool LoadHidding = false;
        public bool IsUILoaded = false;
        public Form CurrentPageView = null;
        private ScrollManager UIScroll = null;

        public GUI.Hardware Hardware_UI;

        public GUI.Antivirus Antivirus_UI;

        public GUI.Optimizer Optimizer_UI;

        public GUI.Cleaner Cleaner_UI;

        public GUI.Tweats Tweats_UI;

        public GUI.Settings Settings_UI;

        public MainWindow()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
            InitializeProcessWatcher();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            try {

                if (LoadHidding == true) { this.Hide(); }
                if (this.Visible == true) { removeToolStripMenuItem.Text = "Show"; } else { removeToolStripMenuItem.Text = "Hide"; }

                LoadTheme();
                LoadUIForms();
                IsUILoaded = true;
                UIScroll = new ScrollManager(PanelContainer, new Control[] { guna2VScrollBar1 }, true);
                HardwareButton.Checked = true;

                if (Program.SplashForm != null && Program.SplashForm.Visible == true)
                {
                    Program.SplashForm.ClosedSplash();
                    Utilities.Sleep(1);
                    this.TransparencyKey = Color.FromArgb(19, 32, 41);
                    guna2Panel1.Visible = true;
                    this.ShowIcon = true;
                    this.ShowInTaskbar = true;
                    guna2ShadowForm1.SetShadowForm(this);
                    Program.SendMessageToExistingInstance(this.Handle);
                    Utilities.Sleep(1);
                    Program.SendMessageToExistingInstance(this.Handle);
                }

            } catch { }
           
        }

        private void LoadUIForms()
        {
            //PanelContainer.SetDoubleBuffered(true);

            if (Global_Instances.Lite == true)
            {

                Hardware_UI = new GUI.Hardware() { TopLevel = false, Text = "0", Visible = false };
                
                var Timer_Hardware_UI = CreateRenderTimer(Hardware_UI);

                Hardware_UI.VisibleChanged += delegate {
                    Timer_Hardware_UI.Enabled = Hardware_UI.Visible;
                };

                Antivirus_UI = new GUI.Antivirus() { TopLevel = false, Text = "1", Visible = false };

                Optimizer_UI = new GUI.Optimizer() { TopLevel = false, Text = "2", Visible = false };

                var Timer_Optimizer_UI = CreateRenderTimer(Optimizer_UI);

                Optimizer_UI.VisibleChanged += delegate
                {
                    Timer_Optimizer_UI.Enabled = Optimizer_UI.Visible;
                };

                Cleaner_UI = new GUI.Cleaner() { TopLevel = false, Text = "3", Visible = false };
                //Cleaner_UI.UpdateRenderData(); // Changed to Cleaner.Load Event

                Tweats_UI = new GUI.Tweats() { TopLevel = false, Text = "4", Visible = false };
                //Tweats_UI.UpdateRenderData(); // Changed to Tweats.Load Event

                Settings_UI = new GUI.Settings() { TopLevel = false, Text = "5", Visible = false };


                PanelContainer.Controls.AddRange(new Control[] { Hardware_UI, Antivirus_UI, Optimizer_UI, Cleaner_UI, Tweats_UI, Settings_UI });

            }
            else {

                PanelContainer.Controls.AddRange(new Control[] { Global_Instances.RenderUI.Hardware_UI, Global_Instances.RenderUI.Antivirus_UI, Global_Instances.RenderUI.Optimizer_UI, Global_Instances.RenderUI.Cleaner_UI, Global_Instances.RenderUI.Tweats_UI, Global_Instances.RenderUI.Settings_UI });

            }


        }

        public void LoadTheme()
        {

            try
            {
                string CurrentTheme = Global_Instances.AppSettings.ReadIni("Settings", "CurrentTheme", "");
                if (System.IO.File.Exists(CurrentTheme))
                {

                    Image PreThemeAssets = Image.FromFile(CurrentTheme);
                    Bitmap PostThemeAssets = Utilities.Resize_Image(PreThemeAssets, guna2Panel1.Width, guna2Panel1.Height);
                    guna2Panel1.BackgroundImage = PostThemeAssets;
                    PreThemeAssets.Dispose();
                } else
                {
                    guna2Panel1.BackgroundImage = null;
                    guna2Panel1.Refresh();
                    guna2Panel1.Update();
                }
            }
            catch { }

        }

        public Label GetFPSLabel() { return FrameRateLabel; }

        private void UINavButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (IsUILoaded == false && this.Visible == false) { return; }

            //PanelContainer.Visible = false;

            if (Global_Instances.Lite == true) {

                Guna.UI2.WinForms.Guna2Button NavButton = (Guna.UI2.WinForms.Guna2Button)sender;

                if (NavButton == HardwareButton)
                {
                    CurrentPageView = Hardware_UI;
                }
                else if (NavButton == AntivirusButton)
                {
                    CurrentPageView = Antivirus_UI;
                }
                else if (NavButton == OptimizerButton)
                {
                    CurrentPageView = Optimizer_UI;
                }
                else if (NavButton == CleanerButton)
                {
                    CurrentPageView = Cleaner_UI;
                }
                else if (NavButton == TweatsButton)
                {
                    CurrentPageView = Tweats_UI;
                }
                else if (NavButton == SettingsButton)
                {
                    CurrentPageView = Settings_UI;
                }

                ChangeToView(CurrentPageView); 

            } 
            else
            {

                Guna.UI2.WinForms.Guna2Button NavButton = (Guna.UI2.WinForms.Guna2Button)sender;

                if (NavButton == HardwareButton)
                {
                    CurrentPageView = Global_Instances.RenderUI.Hardware_UI;
                }
                else if (NavButton == AntivirusButton)
                {
                    CurrentPageView = Global_Instances.RenderUI.Antivirus_UI;
                }
                else if (NavButton == OptimizerButton)
                {
                    CurrentPageView = Global_Instances.RenderUI.Optimizer_UI;
                }
                else if (NavButton == CleanerButton)
                {
                    CurrentPageView = Global_Instances.RenderUI.Cleaner_UI;
                }
                else if (NavButton == TweatsButton)
                {
                    CurrentPageView = Global_Instances.RenderUI.Tweats_UI;
                }
                else if (NavButton == SettingsButton)
                {
                    CurrentPageView = Global_Instances.RenderUI.Settings_UI;
                }

                Global_Instances.RenderUI.ChangeToView(CurrentPageView); // Render FrontEnd App Data

            }

              
            this.UpdateScroll(); // Update Main Scroll
        }

        public void ChangeToView(Form UI)
        {

            UI.BringToFront();

            foreach (Control item in UI.Parent.Controls)
            {
                if (string.Equals(item.Name, UI.Name, StringComparison.OrdinalIgnoreCase) == true) { continue; }
                item.Visible = false;
                item.SendToBack();
            }
            UI.Visible = true;
            UI.Invalidate();
            UI.Refresh();
            UI.Update();
        }

        public void ShowMessage(string Message) {
            this.Invoke(new Action(() =>
            {
                guna2MessageDialog1.Text = Message; guna2MessageDialog1.Show();
            }));
        }

        public void UpdateScroll()
        {
            UIScroll.UpdateScroll();
        }

        private void guna2VScrollBar1_VisibleChanged(object sender, EventArgs e)
        {
            panel1.Visible = guna2VScrollBar1.Visible;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private System.Windows.Forms.Timer CreateRenderTimer(IRenderForm Form) {
           
            System.Windows.Forms.Timer TimerLiteMode = new System.Windows.Forms.Timer();
            TimerLiteMode.Enabled = false;
            TimerLiteMode.Interval = 500;
            TimerLiteMode.Tick += delegate {
                Form.BeginFrame();
                Form.UpdateRenderData();
            };

            return TimerLiteMode;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PerformVisible();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Process.GetCurrentProcess().Kill();
            Environment.Exit(0);
        }

        private void PerformVisible()
        {
            if (this.Visible == true) { removeToolStripMenuItem.Text = "Show"; this.Hide(); } else { removeToolStripMenuItem.Text = "Hide"; this.Show(); }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            removeToolStripMenuItem.Text = "Show";
            this.Hide();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { PerformVisible(); }
        }

        #region " Process Watcher "

        private ProcessWatcher ProcessMon = null;

        private void InitializeProcessWatcher()
        {

            if (ProcessMon == null)
            {
                ProcessMon = new ProcessWatcher();
                ProcessMon.ProcessStatusChanged += ProcessMon_ProcessStatusChanged;
                ProcessMon.Start();
            }


        }

        private void ProcessMon_ProcessStatusChanged(object sender, ProcessWatcher.ProcessStatusChangedEventArgs e)
        {
            switch (e.ProcessEvent)
            {
                case ProcessWatcher.ProcessEvents.Arrival:
                    {
                        try
                        {
                            if (e.ProcessInfo.ProcessName == CurrentProc.ProcessName && int.Parse(e.ProcessID) != CurrentProc.Id)
                            {
                                Win32_Process ProcInfo = e.Win32Info;

                                string[] CommandLines = ProcInfo.CommandLine.Split(" ".ToCharArray().FirstOrDefault());

                                Core.FastArgumentParser FastArgumentParser = new Core.FastArgumentParser();

                                IArgument GameArg = FastArgumentParser.Add("Game").SetDescription("Game FileName");

                                IArgument SilentArg = FastArgumentParser.Add("silent").SetDescription("Start In Silent Mode");

                                FastArgumentParser.Parse(CommandLines);

                                bool IsSilent = CommandLines.ToList().Contains("-silent");

                            if (IsSilent == false && string.IsNullOrEmpty(GameArg.Value) == true)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    if (this.Visible == false) { removeToolStripMenuItem.Text = "Hide"; this.Show(); }
                                }));
                            }
                            e.ProcessInfo.Kill();
                          }
                    } catch { }

                    break;
                    }

                case ProcessWatcher.ProcessEvents.Stopped:
                    {

                        break;
                    }
            }


            #endregion


        }

        private void PanelContainer_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
