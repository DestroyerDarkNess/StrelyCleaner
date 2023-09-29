
using MK.Tools.ForceDel;
using StrelyCleaner.Controls;
using StrelyCleaner.Core;
using StrelyCleaner.Core.Cleaner.Apps;
using StrelyCleaner.Core.Cleaner.Browser;
using StrelyCleaner.Core.Cleaner.Folders;
using StrelyCleaner.Core.Cleaner.Systems;
using StrelyCleaner.Core.Interfaces;
using StrelyCleaner.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using XylonV2.Core.File;

namespace StrelyCleaner.GUI
{
    public partial class Cleaner : Form, IRenderForm
    {
        private bool Initialized = false;
        private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        List<ICleaner> BrowserCleaner = null;
        List<ICleaner> AppsCleaner = null;
        List<ICleaner> SystemCleaner = null;
        List<ICleaner> FoldersCleaner = null;
        private ScrollManager panelFX2Scroll = null;

        Dictionary<ICleanerOption, List<InfoFile>> FilesCleanData = new Dictionary<ICleanerOption, List<InfoFile>>();

        public Cleaner()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            this.BackColor = Color.Transparent;

            panelFX2Scroll = new ScrollManager(panelFX1, new Control[] { guna2VScrollBar1 }, true);
        }

        public void BeginFrame()
        {
            if (BrowserGroupBox1.Visible == false && BrowserContainer.Controls.Count == 0) { if (BrowserCleaner != null) { BrowserGroupBox1.Visible = true; ListBrowserOptions(); } }

            if (ApplicationsGroupBox.Visible == false && ApplicationsContainer.Controls.Count == 0) { if (AppsCleaner != null) { ApplicationsGroupBox.Visible = true; ListAppsOptions(); } }

            if (SystemGroupBox.Visible == false && SystemContainer.Controls.Count == 0) { if (SystemCleaner != null) { SystemGroupBox.Visible = true; ListSystemOptions(); } }

            if (FoldersGroupBox.Visible == false && FoldersContainer.Controls.Count == 0) { if (FoldersCleaner != null) { FoldersGroupBox.Visible = true; ListFoldersOptions(); } }

            if (Global_Instances.ProcessProvider != null)
            {
                if (Global_Instances.ProcessProvider.Enabled == true) { Global_Instances.ProcessProvider.Enabled = false; }
            }

            if (BrowserGroupBox1.Visible == true && ApplicationsGroupBox.Visible == true &&
                SystemGroupBox.Visible == true && FoldersGroupBox.Visible == true && guna2Button1.Visible == false)
            {
                guna2Button1.Visible = true;
            }
        }

        public void UpdateRenderData()
        {
            if (Initialized == false) {
                Initialized = true;

                BrowserCleaner = new List<ICleaner> { new Edge(), new IExplorer(), new Opera(), new Chrome() };
                AppsCleaner = new List<ICleaner> { new UWP() };
                SystemCleaner = new List<ICleaner> { new Temp(), new CrashDumps() };
                FoldersCleaner = new List<ICleaner> { new Downloads() };
                if (Global_Instances.Lite == true)
                {
                    this.Invoke(new Action(() =>
                    {
                        BeginFrame();
                    }));
                }
            }
        }

        private void Cleaner_Load(object sender, EventArgs e)
        {
            //foreach (Core.InstalledProgram Program in Core.InstalledProgramManager.GetInstalledPrograms()) {

            //    if (System.IO.Directory.Exists(Program.InstallLocation) != true) { continue; }

            //    if (Program.Publisher.ToLower() != "microsoft corporation") { listBox1.Items.Add(Program.Name); }

            //    //string SignerName = String.Empty;

            //    //if (ProcessHacker.Native.Cryptography.VerifyFile(Program.InstallLocation, out SignerName) != ProcessHacker.Native.VerifyResult.NoSignature) {

            //    //} else { listBox1.Items.Add(Program.Name); }

            //}
            if (Global_Instances.Lite == true) { this.UpdateRenderData(); }
        }




        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                if (Fix == false) { Analize(); } else { Clean(); }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
           
        }

        private void guna2Button1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2ProgressBar1.Value = 0;
            listView1.Items.Clear();
            DataPanel.SendToBack();
            DataHeaderPanel.Visible = false;
            guna2ProgressBar1.Visible = true;
            DataPanel.Visible = false;
            guna2Button2.Visible = false;
            guna2Button1.Text = "Analyze";
            Fix = false;
        }

        private bool Fix = false;
        private string TotalSpaceToClean = string.Empty;
        private void Analize() {

            this.Invoke(new Action(() =>
            {
                guna2Button2.Visible = false;
                guna2Button1.Enabled = false;
                guna2Button1.Text = "Analyzing...";
            }));

           

            watch.Start();

            FilesCleanData.Clear();
            List < ICleaner > AllOptions = new List < ICleaner >();
            AllOptions.AddRange(BrowserCleaner);
            AllOptions.AddRange(AppsCleaner);
            AllOptions.AddRange(SystemCleaner);
            AllOptions.AddRange(FoldersCleaner);

            foreach (ICleaner Cleaner in  AllOptions)
            {

                foreach (ICleanerOption Option in Cleaner.GetOptions)
                {

                    if (Option.Enabled == true)
                    {

                        switch (Option.Type)
                        {
                            case CleanOptionType.File:

                                List<InfoFile> NewFileList = new List<InfoFile>();
                                foreach (string file in Option.Data())
                                {
                                    if (System.IO.File.Exists(file) == true) { NewFileList.Add(new InfoFile(file)); }
                                }

                                FilesCleanData.Add(Option, NewFileList);

                                break;
                            case CleanOptionType.Directory:
                                // code block
                                break;
                            case CleanOptionType.Registry:

                            default:
                                // code block
                                break;
                        }

                    }
                }

            }

            this.Invoke(new Action(() =>
            {
                guna2Button3.Visible = false;
                listView1.Visible = false;
                panelFX1.BringToFront();
                panelFX1.Visible = true;
                guna2VScrollBar1.Visible = true;
                guna2ProgressBar1.Value = 0;
                guna2ProgressBar1.Maximum = FilesCleanData.Count;
                listView1.Items.Clear();
                DataPanel.BringToFront();
                DataHeaderPanel.Visible = false;
                guna2ProgressBar1.Visible = true;
                DataPanel.Height = 326;
                DataPanel.Visible = true;
                panelFX1.Controls.Clear();
                TotalSpaceToClean = string.Empty;
            }));

            long TotalSumFiles = 0;

            ControlLister Listener_Clean = new ControlLister { OrientationControls = Orientation.Vertical, Margen = new Point(10, 10) };

            List<ICleanerOption> IndexSearch = new List<ICleanerOption>(FilesCleanData.Keys);

            foreach (KeyValuePair<ICleanerOption, List<InfoFile>> CleanerData in FilesCleanData)
            {
                try {

                    int Index = IndexSearch.IndexOf(CleanerData.Key);

                    guna2ProgressBar1.Value += 1;

                    var items = CleanerData.Value.Select(item => item.FullName.ToString()).OrderBy(x => x);
                    long SumFiles = CleanerData.Value.Sum(item => long.Parse(item.FileSize_Byte.Replace(".", "")));
                    TotalSumFiles += SumFiles;

                    this.Invoke(new Action(() =>
                    {
                        CustomItemClick ItemClick = new CustomItemClick();
                        ItemClick.Name = CleanerData.Key.Parent + "_" + CleanerData.Key.id;
                        ItemClick.SetText($" {CleanerData.Key.Parent} - {CleanerData.Key.id} ");
                        ItemClick.SetSubInfo1($"{StrelyCleaner.Helpers.Functions.Round_Bytes(SumFiles)}");
                        ItemClick.SetSubInfo2($"{CleanerData.Value.Count.ToString("N0")} Files  ");

                      
                        ICleaner Parent = AllOptions.OfType<ICleaner>().FirstOrDefault(item => item.id == CleanerData.Key.Parent);

                        if (Parent != null) { ItemClick.SetIcon(Parent.icon); }

                        ItemClick.Click += (sender, e) =>
                        {
                            List<ListViewItem> listViewItems = CleanerData.Value
                                 .Select(infoFile => new ListViewItem(new[] { infoFile.FullName.ToString(), double.Parse(infoFile.FileSize_MB).ToString("0.00") + "mb" }))
                                 .ToList();
                            listView1.Items.Clear();

                            listView1.Items.AddRange(listViewItems.ToArray());

                            panelFX1.Visible = false;
                            guna2VScrollBar1.Visible = false;
                            guna2Button3.Visible = true;
                            listView1.BringToFront();
                            listView1.Visible = true;
                        };

                        ItemClick.MouseHover += (sender, e) =>
                        {
                            ItemClick.BackColor = Color.FromArgb(29, 29, 31);
                        };

                        ItemClick.MouseLeave += (sender, e) =>
                        {
                            ItemClick.BackColor = Color.Transparent;
                        };

                        Listener_Clean.Add(panelFX1, ItemClick);
                    }));

                   

                } catch { }

               
            }

            watch.Stop();

            double segundos = (double)watch.ElapsedMilliseconds / 1000.0;
            TotalSpaceToClean = StrelyCleaner.Helpers.Functions.Round_Bytes(TotalSumFiles);

            this.Invoke(new Action(() =>
            {
                label2.Text = $"Scan Completed - ({segundos.ToString("0.00")}) seconds";
                label4.Text = $"{TotalSpaceToClean} to clean";

                guna2ProgressBar1.Visible = false;
                DataHeaderPanel.Visible = true;
                guna2Button1.Text = "Clean";
                guna2Button1.Enabled = true;
                guna2Button2.Visible = true;
            }));

           
            Fix = true;
        }

        private void Clean() {

            this.Invoke(new Action(() =>
            {
                guna2Button1.Visible = false;
                guna2Button2.Visible = false;
                guna2ProgressBar1.Visible = true;
                DataHeaderPanel.Visible = false;
            }));

            foreach (KeyValuePair<ICleanerOption, List<InfoFile>> CleanerData in FilesCleanData)
            {
                CustomItemClick ItemClean = panelFX1.Controls.OfType<CustomItemClick>().FirstOrDefault(control => control.Name == CleanerData.Key.Parent + "_" + CleanerData.Key.id);

                this.Invoke(new Action(() => {
                    if (ItemClean != null)
                    {
                        ItemClean.Enabled = false;
                        ItemClean.BackColor = Color.Orange;
                    }
                }));

                guna2ProgressBar1.Value = 0;
                guna2ProgressBar1.Maximum = CleanerData.Value.Count;
              
                  foreach (InfoFile File in CleanerData.Value) {
                      try
                       {
                        guna2ProgressBar1.Value += 1;
                        if (System.IO.File.Exists(File.FullName) == true) { System.IO.File.Delete(File.FullName); }
                   
                       }
                    catch  { if (Core.Settings.Experimental) new FileDeleter().Delete(File.FullName); }
                  }

                this.Invoke(new Action(() => { guna2ProgressBar1.Invalidate();

                    if (ItemClean != null) {
                        ItemClean.Enabled = false;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(ItemClean.Name + " 100% Completed!");
                        Console.ForegroundColor = ConsoleColor.White;
                        ItemClean.BackColor = Color.LimeGreen; }

                }));
            }

            this.Invoke(new Action(() =>
            {
                guna2ProgressBar1.Value = 0;
                listView1.Items.Clear();
                DataPanel.SendToBack();
                DataHeaderPanel.Visible = false;
                guna2ProgressBar1.Visible = true;
                DataPanel.Visible = false;
                guna2Button2.Visible = false;
                guna2Button1.Text = "Analyze";
                guna2Button1.Visible = true;
                Fix = false;
            }));

            if (this.Visible == true) Global_Instances.MainUI.ShowMessage($"{TotalSpaceToClean} of your PC has been cleaned.");
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            guna2Button3.Visible = false;
            listView1.Visible = false;
            panelFX1.BringToFront();
            panel1.BringToFront();
            panelFX1.Visible = true;
            guna2VScrollBar1.Visible = true;
            panelFX2Scroll.UpdateScroll();
        }

        #region " Browsers "

        private void ListBrowserOptions()
        {

            ControlLister Listener_Browsers = new ControlLister { OrientationControls = Orientation.Vertical, Margen = new Point(10, 10) };

            foreach (ICleaner CleanerOpt in BrowserCleaner)
            {

                OptionContainer optionContainer = new OptionContainer() { Size = new Size(620,19) };
                optionContainer.SetTitle(CleanerOpt.id);

                foreach (ICleanerOption Option in CleanerOpt.GetOptions)
                {
                   
                    Guna.UI2.WinForms.Guna2CheckBox CheckControl = CreateCheckControl(Option.id);
                    CheckControl.CheckedChanged += delegate { Option.Enabled = CheckControl.Checked; }; 
                    CheckControl.Checked = true;

                    optionContainer.AddCheck(CheckControl, true);
                   
                }

                Listener_Browsers.Add(BrowserContainer, optionContainer);
            }


        }


        private void BrowserButton_CheckedChanged(object sender, EventArgs e)
        {
            if (BrowserButton.Checked) { BrowserGroupBox1.Height = 300; } else { BrowserGroupBox1.Height = BrowserButton.Height; }
        }


        #endregion

        #region " Apps "

        private void ListAppsOptions()
        {

            ControlLister Listener_Appas = new ControlLister { OrientationControls = Orientation.Vertical, Margen = new Point(10, 10) };

            foreach (ICleaner CleanerOpt in AppsCleaner)
            {

                OptionContainer optionContainer = new OptionContainer() { Size = new Size(620, 19) };
                optionContainer.SetTitle(CleanerOpt.id);

                foreach (ICleanerOption Option in CleanerOpt.GetOptions)
                {

                    Guna.UI2.WinForms.Guna2CheckBox CheckControl = CreateCheckControl(Option.id);
                    CheckControl.CheckedChanged += delegate { Option.Enabled = CheckControl.Checked; };
                    CheckControl.Checked = true;

                    optionContainer.AddCheck(CheckControl, true);

                }

                Listener_Appas.Add(ApplicationsContainer, optionContainer);
            }


        }


        private void ApplicationButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ApplicationButton.Checked) { ApplicationsGroupBox.Height = 300; } else { ApplicationsGroupBox.Height = ApplicationButton.Height; }
        }

        #endregion

        #region " System "

        private void ListSystemOptions()
        {

            ControlLister Listener_System = new ControlLister { OrientationControls = Orientation.Vertical, Margen = new Point(10, 10) };

            foreach (ICleaner CleanerOpt in SystemCleaner)
            {

                OptionContainer optionContainer = new OptionContainer() { Size = new Size(620, 19) };
                optionContainer.SetTitle(CleanerOpt.id);

                foreach (ICleanerOption Option in CleanerOpt.GetOptions)
                {

                    Guna.UI2.WinForms.Guna2CheckBox CheckControl = CreateCheckControl(Option.id);
                    CheckControl.CheckedChanged += delegate { Option.Enabled = CheckControl.Checked; };
                    CheckControl.Checked = true;

                    optionContainer.AddCheck(CheckControl, true);

                }

                Listener_System.Add(SystemContainer, optionContainer);
            }


        }

        private void SystemButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SystemButton.Checked) { SystemGroupBox.Height = 300; } else { SystemGroupBox.Height = SystemButton.Height; }
        }

        #endregion

        #region " Folders "

        private void ListFoldersOptions()
        {

            ControlLister Listener_Folders = new ControlLister { OrientationControls = Orientation.Vertical, Margen = new Point(10, 10) };

            foreach (ICleaner CleanerOpt in FoldersCleaner)
            {

                OptionContainer optionContainer = new OptionContainer() { Size = new Size(620, 19) };
                optionContainer.SetTitle(CleanerOpt.id);

                foreach (ICleanerOption Option in CleanerOpt.GetOptions)
                {

                    Guna.UI2.WinForms.Guna2CheckBox CheckControl = CreateCheckControl(Option.id);
                    CheckControl.CheckedChanged += delegate { Option.Enabled = CheckControl.Checked; };
                    //CheckControl.Checked = true;

                    optionContainer.AddCheck(CheckControl, true);

                }

                Listener_Folders.Add(FoldersContainer, optionContainer);
            }


        }


        private void FoldersButton_CheckedChanged(object sender, EventArgs e)
        {
            if (FoldersButton.Checked) { FoldersGroupBox.Height = 300; } else { FoldersGroupBox.Height = FoldersButton.Height; }
        }

        #endregion

        #region " Private Methods "

        private Guna.UI2.WinForms.Guna2CheckBox CreateCheckControl(String Text)
        {

            Guna.UI2.WinForms.Guna2CheckBox CustomCheckBox = new Guna.UI2.WinForms.Guna2CheckBox();
          
            CustomCheckBox.Font = new Font("Arial", 12, FontStyle.Bold);
            CustomCheckBox.UncheckedState.BorderRadius = 3;
            CustomCheckBox.CheckedState.BorderRadius = 3;
            CustomCheckBox.CheckedState.FillColor = System.Drawing.Color.DodgerBlue;
            CustomCheckBox.UncheckedState.FillColor = System.Drawing.Color.Gray;
            CustomCheckBox.Name = Text;
            CustomCheckBox.Text = " " + Text;

            CustomCheckBox.Size = new System.Drawing.Size(111, 17);

            return CustomCheckBox;
        }







        #endregion

       
    }
}
