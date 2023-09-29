using Guna.UI2.WinForms;
using ProcessHacker.Native.Api;
using StrelyCleaner.Controls;
using StrelyCleaner.Core;
using StrelyCleaner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XylonV2;

namespace StrelyCleaner.GUI
{
    public partial class Settings : Form, IRenderForm
    {
        public Settings()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            this.BackColor = Color.Transparent;
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
           
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string ThemeFile = openFileDialog1.FileName;
            if (System.IO.File.Exists(ThemeFile) == true)
            {
                try
                {
                    IEnumerable<string> ThemeList = FileDirSearcher.GetFilePaths(dirPath: Global_Instances.AppThemeFolder, searchOption: SearchOption.TopDirectoryOnly);
                    string ToCopyPath = System.IO.Path.Combine(Global_Instances.AppThemeFolder, ThemeList.Count() + System.IO.Path.GetFileName(ThemeFile));
                    System.IO.File.Copy(ThemeFile, ToCopyPath, true);

                    LoadAllThemes();
                }
                catch { }
            }
        }

        ControlLister Listener_Theme = new ControlLister { OrientationControls = Orientation.Horizontal, Margen = new Point(5, 5) };
        private void LoadAllThemes()
        {
            RemoveAll();
            panel2.Parent = this;

            IEnumerable<string> ThemeList = FileDirSearcher.GetFilePaths(dirPath: Global_Instances.AppThemeFolder, searchOption: SearchOption.TopDirectoryOnly, null, fileExtPatterns: new string[]
   {
                "*.bmp",  "*.jpg", "*.jpeg","*.png", "*.tif", "*.tiff"
   }, ignoreCase: true, throwOnError: false);

            Listener_Theme.Add(panelFX1, panel2, false);

            Panel NoImageItem = new Panel();
            NoImageItem.BackColor = Color.FromArgb(19, 19, 21);
            NoImageItem.BorderStyle = BorderStyle.FixedSingle;
            NoImageItem.Tag = "";
            NoImageItem.Size = new Size(129, 67);
            NoImageItem.BackgroundImageLayout = ImageLayout.Stretch;
            NoImageItem.Cursor = Cursors.Hand;
            NoImageItem.Click += (s, e) => {
                Global_Instances.AppSettings.WriteIni("Settings", "CurrentTheme", "");
                Utilities.Sleep(1);
                Global_Instances.MainUI.LoadTheme();
            };

            Listener_Theme.Add(panelFX1, NoImageItem, true);

            foreach (string GameFile in ThemeList)
            {

                if (System.IO.File.Exists(GameFile))
                {

                    try
                    {

                        Panel ImageItem = new Panel();
                        ImageItem.Tag = GameFile;
                        ImageItem.Size = new Size(129, 67); 
                        ImageItem.BackgroundImageLayout = ImageLayout.Stretch;

                        Image PreThemeAssets = Image.FromFile(GameFile);
                        Bitmap PostThemeAssets = Utilities.Resize_Image(PreThemeAssets, ImageItem.Width, ImageItem.Height);
                        ImageItem.BackgroundImage = PostThemeAssets;
                        PreThemeAssets.Dispose();

                        ImageItem.ContextMenuStrip = guna2ContextMenuStrip1;
                        ImageItem.Cursor = Cursors.Hand;
                        ImageItem.Click += (s, e) => {
                            Global_Instances.AppSettings.WriteIni("Settings", "CurrentTheme", GameFile.ToString());
                            Utilities.Sleep(1);
                            Global_Instances.MainUI.LoadTheme();
                        };

                        Listener_Theme.Add(panelFX1, ImageItem, true);

                    }
                    catch { }

                }

            }

        }

        private bool RemoveAll()
        {
            panel2.Parent = this;
            //foreach (Control ControlEx in panelFX1.Controls)
            //{
            //    //if ( string.Equals(ControlEx.Name, panel2.Name, StringComparison.OrdinalIgnoreCase) == false ) { 
            //    panelFX1.Controls.Remove(ControlEx);
            //    //}
            //}
            panelFX1.Controls.Clear();
            return true;
        }


        private void label7_Click(object sender, EventArgs e)
        {

        }

       private  bool SettingsLoaded = false;

        private void Settings_Load(object sender, EventArgs e)
        {
            guna2ToggleSwitch6.Checked = Global_Instances.Lite;

            if (Global_Instances.Lite == true) {
                guna2Panel1.FillColor = Color.DimGray;
                guna2Panel2.FillColor = Color.DimGray;
                guna2Panel3.FillColor = Color.DimGray;
                guna2Panel7.FillColor = Color.DimGray;
                guna2Panel8.FillColor = Color.DimGray;
                guna2Panel9.FillColor = Color.DimGray;
                guna2ToggleSwitch1.Enabled = false;
                guna2ToggleSwitch2.Enabled = false;
                guna2ToggleSwitch3.Enabled = false;
                guna2ToggleSwitch4.Enabled = false;
                guna2ToggleSwitch5.Enabled = false;
                guna2NumericUpDown1.Enabled = false;
            } 
            else
            {

                guna2ToggleSwitch1.Checked = Boolean.Parse(Global_Instances.AppSettings.ReadIni("Settings", "Persistence", "False"));
                guna2ToggleSwitch2.Checked = (Global_Instances.RenderUI.Global_App_Services != null);
                guna2ToggleSwitch3.Checked = Core.Settings.Experimental;
                guna2ToggleSwitch4.Checked = Boolean.Parse(Global_Instances.AppSettings.ReadIni("Settings", "LockFramesPerSecond", "True"));
                guna2ToggleSwitch5.Checked = Boolean.Parse(Global_Instances.AppSettings.ReadIni("Settings", "VSync", "True"));

                int ValThreads = int.Parse(Global_Instances.AppSettings.ReadIni("Settings", "BackEndThreads", "1"));
                guna2NumericUpDown1.Value = ValThreads;

            }
          

            LoadAllThemes();
            SettingsLoaded = true;
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (SettingsLoaded == false) { return; }
            Global_Instances.AppSettings.WriteIni("Settings", "Persistence", guna2ToggleSwitch1.Checked.ToString());
            Persistence.TaskService(guna2ToggleSwitch1.Checked);
        }

        private void guna2ToggleSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            if (SettingsLoaded == false) { return; }
            Global_Instances.AppSettings.WriteIni("Settings", "AppService", guna2ToggleSwitch2.Checked.ToString());
        }

        private void guna2ToggleSwitch3_CheckedChanged(object sender, EventArgs e)
        {
            if (SettingsLoaded == false) { return; }
            Global_Instances.AppSettings.WriteIni("Settings", "ExperimentalMode", guna2ToggleSwitch3.Checked.ToString());
            Core.Settings.Experimental = guna2ToggleSwitch3.Checked;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    Control sourceControl = owner.SourceControl;
                    MessageBox.Show(sourceControl.Tag.ToString());
                }
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/paypalme/SalvadorKrilewski");
        }

        private void guna2Panel5_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/paypalme/SalvadorKrilewski");
        }

        private void guna2Panel6_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.buymeacoffee.com/s4lsalsoft");
        }

        private void guna2ToggleSwitch4_CheckedChanged(object sender, EventArgs e)
        {
            if (SettingsLoaded == false) { return; }
            Global_Instances.AppSettings.WriteIni("Settings", "LockFramesPerSecond", guna2ToggleSwitch4.Checked.ToString());
            Global_Instances.RenderUI.LockFramesPerSecond = guna2ToggleSwitch4.Checked;
        }

        private void guna2ToggleSwitch5_CheckedChanged(object sender, EventArgs e)
        {
            if (SettingsLoaded == false) { return; }
            Global_Instances.AppSettings.WriteIni("Settings", "VSync", guna2ToggleSwitch5.Checked.ToString());
            Global_Instances.RenderUI.VSync = guna2ToggleSwitch5.Checked;
        }

        private void guna2NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (SettingsLoaded == false) { return; }
            int ValThreads = (int)guna2NumericUpDown1.Value;
            Global_Instances.AppSettings.WriteIni("Settings", "BackEndThreads", ValThreads.ToString());
            Global_Instances.RenderUI.BackEndThreads = ValThreads;
        }

        bool NoCheck = false;
        private void guna2ToggleSwitch6_CheckedChanged(object sender, EventArgs e)
        {
            if (SettingsLoaded == false || NoCheck == true) { NoCheck = false; return; }
            if (MessageBox.Show("This action requires restarting the application. Do you want to continue?", "Lite Mode", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                string AppPath = Application.ExecutablePath;
                Global_Instances.AppSettings.WriteIni("Settings", "LiteMode", guna2ToggleSwitch6.Checked.ToString());
                //Utilities.RunCommand($"timeout /t 2 & start { Utilities.AddDoubleQuotes(AppPath) } ");
                Process.Start(AppPath);
                Process.GetCurrentProcess().Kill();
                Environment.Exit(0);
            }
            else { NoCheck = true; guna2ToggleSwitch6.Checked = !guna2ToggleSwitch6.Checked; }
        }
    }
}
