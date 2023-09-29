using Guna.UI2.WinForms;
using Microsoft.VisualBasic;
using ProcessHacker.Native.Objects;
using ProcessHacker.Native;
using StrelyCleaner.Core;
using StrelyCleaner.Core.Interfaces;
using StrelyCleaner.Core.Optimizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProcessHacker.Native.Security;

namespace StrelyCleaner.GUI.SubForms
{
    public partial class GameLauncher : Form , ITempMainForm
    {
        public static string IniFile = string.Empty;
        public static SettingProvider GameSettings = null;
        public SentinelGameLauncher SGameLauncher = null;
        public Image GameIcon = null;
        public string Shortcut = string.Empty;
        public Process GameLauncherProc = Process.GetCurrentProcess();
        public bool DialogAlive = true;

         Func<bool> ITempMainForm.MaintainThread { get => () => DialogAlive;}

        public GameLauncher( string GamePath)
        {
            SGameLauncher = new SentinelGameLauncher(GamePath);
            InitializeComponent();
            GameIcon = IconExtractor.ExtractIconFromFile(GamePath);
            Shortcut = System.IO.Path.GetFileNameWithoutExtension(GamePath) + "_Sentinel";
            IniFile = System.IO.Path.Combine(Global_Instances.AppFolder, Shortcut + ".ini");
            GameSettings = new SettingProvider(IniFile);
        }

        private void GameLauncher_Load(object sender, EventArgs e)
        {
            Utilities.EssentialProcessNames.Add(System.IO.Path.GetFileNameWithoutExtension(GameLauncherProc.ProcessName));
        }

        private void GameLauncher_Shown(object sender, EventArgs e)
        {
            try {
                guna2TextBox1.Text = GameSettings.ReadIni("Game", "Arguments", string.Empty);
                guna2CheckBox1.Checked = SGameLauncher.IsShortcutOnDesktop(Shortcut);
                guna2CheckBox2.Checked = Boolean.Parse(GameSettings.ReadIni("Game", "ShowAgainOnCloseGame", "False"));
                guna2CheckBox3.Checked = Boolean.Parse(GameSettings.ReadIni("Game", "FakeFullScreen", "False"));
                guna2CheckBox5.Checked = Boolean.Parse(GameSettings.ReadIni("Game", "WindowMode", "False"));
                guna2CheckBox6.Checked = Boolean.Parse(GameSettings.ReadIni("Game", "SuspendProcess", "False"));
                this.Text = Shortcut;
                panel1.BackgroundImage = GameIcon;
                label1.Text = this.Text;
                label2.Text = SGameLauncher.GamePath;
                this.Icon = GameIcon.ToIcon(40, 40);
            } catch { }
           
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2Button1.Enabled = false;

            GameSettings.WriteIni("Game", "Arguments", guna2TextBox1.Text);
            SGameLauncher.CommandLineOptions = guna2TextBox1.Text;

            if (guna2CheckBox5.Checked == true && SGameLauncher.CommandLineOptions.ToLower().Contains("-window") == false) { SGameLauncher.CommandLineOptions += " -window -windowed "; }

            guna2Button1.Text = "Boosting...";
            Utilities.Sleep(1);
            Clean();
             
            
            Game CurrentGame = SGameLauncher.Launch();
            if (CurrentGame != null) {
                bool ReShow = guna2CheckBox2.Checked;
                bool ProcessSuspender = guna2CheckBox6.Checked;
                guna2Button1.Text = "Launching...";
                Utilities.Sleep(1);
                this.Hide();
                Thread t = new Thread(() =>
                {
                    bool Runtime = true;
                    int Seconds = 0;
                    while (Runtime)
                    {
                        if (CurrentGame.IsRunning() == true) {
                            if (Seconds >= 60) {
                                Seconds = 0;
                                Int64 phav = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
                                Int64 tot = PerformanceInfo.GetTotalMemoryInMiB();
                                decimal percentFree = ((decimal)phav / (decimal)tot) * 100;
                                decimal percentOccupied = 100 - percentFree;
                                if (Math.Round(percentOccupied) > Global_Instances.RAMPercent)
                                {
                                    Process[] process = Process.GetProcesses();
                                    foreach (Process p in process)
                                    {
                                        try
                                        {
                                            WinAPI.EmptyWorkingSet(p.Handle);
                                        }
                                        catch { }
                                    }
                                }
                            }

                            if (ProcessSuspender == true) {

                                Process CurrentProces = Utilities.GetForegroundProcess();

                                if (CurrentProces != null && CurrentProces.Id == CurrentGame.processId)
                                {
                                    foreach (Process p in Process.GetProcesses())
                                    {
                                        try
                                        {
                                            if (p.Id != GameLauncherProc.Id && p.Id != CurrentGame.processId && Utilities.IsEssentialProcess(System.IO.Path.GetFileNameWithoutExtension(p.ProcessName)) == false) //&&  Utilities.IsProcessWithoutWindow(p.Id) == true
                                            {

                                                ProcessHandle processHandle = new ProcessHandle(p.Id, ProcessAccess.SuspendResume);

                                                string SignerName = String.Empty;
                                                ProcessHacker.Native.Cryptography.VerifyFile(processHandle.GetImageFileNameWin32(), out SignerName);
                                               
                                                if (SignerName.ToLower() != "microsoft corporation")    {
                                                    processHandle.Suspend();
                                                    processHandle.Dispose();
                                                }
                                             
                                            }
                                        }
                                        catch { }
                                    }

                                }
                                else
                                {
                                    foreach (Process p in Process.GetProcesses())
                                    {
                                        try
                                        {
                                            ProcessHandle processHandle = new ProcessHandle(p.Id, ProcessAccess.SuspendResume);
                                            processHandle.Resume();
                                            processHandle.Dispose();
                                        }
                                        catch { }
                                    }
                                }

                            }
                            

                            Utilities.Sleep(1, Utilities.Measure.Seconds);
                            Seconds += 1;
                        } else {

                            if (ProcessSuspender == true)
                            {
                                foreach (Process p in Process.GetProcesses())
                                {
                                    try
                                    {
                                        ProcessHandle processHandle = new ProcessHandle(p.Id, ProcessAccess.SuspendResume);
                                        processHandle.Resume();
                                        processHandle.Dispose();
                                    }
                                    catch { }
                                }
                            }
                               
                            Runtime = false;
                        }
                    }

                    this.Invoke(new Action(() =>
                    {
                        if (ReShow == true) {
                           guna2Button1.Text = "Launch"; this.Show(); guna2Button1.Enabled = true; 
                        } else { DialogAlive = false; this.Close(); }
                    }));

                });

                t.Priority = ThreadPriority.Highest ;    
                t.Start();
            }
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked) {
                if (SGameLauncher.IsShortcutOnDesktop(Shortcut) == false) { SGameLauncher.CreateShortcutOnDesktop(Shortcut); }
            } else {
                if (SGameLauncher.IsShortcutOnDesktop(Shortcut) == true) { System.IO.File.Delete(SGameLauncher.GetShortcutPath(Shortcut)); }
            }
        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            GameSettings.WriteIni("Game", "ShowAgainOnCloseGame", guna2CheckBox2.Checked.ToString());
        }

        private void guna2CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Height == 190) {
                panelFX1.Visible = true;
                guna2VScrollBar1.Visible = true;
                this.Height = 480;
            } else {
                panelFX1.Visible = false;
                guna2VScrollBar1.Visible = false;
                this.Height = 190;
            }
        }


        private void Clean() {

            Process[] process = Process.GetProcesses();
            foreach (Process p in process) try { WinAPI.EmptyWorkingSet(p.Handle); } catch { }
            DirectoryInfo directory = new DirectoryInfo(Path.GetTempPath());
            foreach (FileInfo file in directory.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch { }
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch { }
            }
            directory = new DirectoryInfo(Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine));
            foreach (FileInfo file in directory.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch { }
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch { }
            }

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            DialogAlive = false;
        }

        private void guna2CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            SGameLauncher.FakeFullScreen = guna2CheckBox3.Checked;
            GameSettings.WriteIni("Game", "FakeFullScreen", guna2CheckBox3.Checked.ToString());
            if (guna2CheckBox3.Checked == true && guna2CheckBox5.Checked == false) { guna2CheckBox5.Checked = true; } 
        }

        private void guna2CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            SGameLauncher.WindowMode = guna2CheckBox5.Checked;
            GameSettings.WriteIni("Game", "WindowMode", guna2CheckBox5.Checked.ToString());
        }


        private void guna2CheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            GameSettings.WriteIni("Game", "SuspendProcess", guna2CheckBox6.Checked.ToString());
        }
    }
}
