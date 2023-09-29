using dnlib.DotNet;
using Guna.UI2.WinForms;
using IWshRuntimeLibrary;
using MK.Tools.ForceDel;
using ProcessHacker.Native.Api;
using StrelyCleaner.Core;
using StrelyCleaner.Core.Antivirus;
using StrelyCleaner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XylonV2;
using XylonV2.Core.Helper;
using XylonV2.Engine.External.WindowsDefender;
using XylonV2.Engine.Pinvoke;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StrelyCleaner.GUI.SubForms
{

    public enum ScanType : int
    {
        Full = 0,
        Smart = 1,
        Custom = 2,
        USB = 3
    }

    public partial class AVScanner : Form
    {
        public string CustomScanPath = string.Empty;
        public ScanType CustomScanType = ScanType.Smart;

        private IScan StartupList = new Startup();

        public AVScanner(ScanType customScanType)
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            CustomScanType = customScanType;
        }

        private void AVScanner_Load(object sender, EventArgs e)
        {
            listView1.OwnerDraw = true;

        }
        private void AVScanner_Shown(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {


                switch (CustomScanType)
                {
                    case ScanType.Full:
                        FullScan();
                        break;
                    case ScanType.Smart:
                        SmartScan();
                        break;
                    case ScanType.Custom:
                        CustomScan(CustomScanPath);
                        break;
                    case ScanType.USB:
                       
                        if ( NoAnalize == true)
                        {
                            if (FixDevice == true)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    guna2ProgressBar1.Style = ProgressBarStyle.Marquee;
                                    label2.Text = "Preparing USB Device, this may take several minutes, please wait....";
                                }));

                                DoRepairUSB();
                            }
                            else { Solve(); }
                        }
                        else { USBScan(CustomScanPath); }

                        break;
                    default:

                        break;
                }


            });

            t.Start();
        }

        #region " UI "

        private bool Stop = false;

        private  void guna2Button1_Click(object sender, EventArgs e)
        {
            if (Stop == false)
            {
                Stop = true;
            }
            if (guna2Button1.Checked == true) { 
                
                guna2Button1.Enabled = false;
                
                if (listView1.Items.Count != 0)
                {
                    if (CustomScanType == ScanType.USB && FixDevice == true)
                    {
                        guna2ProgressBar1.Style = ProgressBarStyle.Marquee;
                        label2.Text = "Preparing USB Device, this may take several minutes, please wait....";
                        DoRepairUSB();
                    }
                    else { Solve(); }
                }
            }
        }

        public bool FixDevice = false;
        public bool NoAnalize = false;

        private void DoRepairUSB()
        {
            Thread t = new Thread(() =>
            {
                string RepairFix = Utilities.RunConsole("chkdsk.exe", $"/f /x {CustomScanPath}");
                Utilities.Sleep(5);
                this.Invoke(new Action(() =>
                {
                    guna2ProgressBar1.Style = ProgressBarStyle.Blocks;
                    label2.Text = "Disinfecting device...";
                    Utilities.Sleep(4);
                    Solve(RepairFix);
                }));
            });

            t.Start();
          
        }


        private  void Solve(string RepairFix = "") {

            Antivirus Parent = this.ParentForm as Antivirus;

            int ThreadsCount = 0;
            
            string[] SuspiciusProcess =
   {
        "cmd", "rundll32", "wscript", "cscript", "showmyhey", "microsoft", "msiexec", "mspaint", "mscalc", "calc", "autoit3"};

            foreach (string ProcName in SuspiciusProcess)
            {
                KillProcByName(ProcName);
            }

            if (NoAnalize == true) {

                IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: CustomScanPath, searchOption: SearchOption.TopDirectoryOnly, null, fileExtPatterns: new string[]
                {
                 "*.cmd", "*.com", 
                 "*.js","*.vbscript", "*.vbs",
                 "*.wsf", "*.py", "*.ps1", "*.lnk", "*.reg", "*.rgs", "*.run",
                 "*.wsh", "*.hta", "*.vbe", "*.scr", "*.chk"
                }, ignoreCase: true, throwOnError: false);

                foreach (string File in Files) {
                    AV_File AvFile = new AV_File(File, "GenericDetection.USB/Malware");

                    this.Invoke(new Action(() =>
                    {
                        ListViewItem item = new ListViewItem(AvFile.FileName);
                        item.SubItems.Add("File");
                        item.SubItems.Add(AvFile.Id);
                        item.Checked = true;

                        listView1.Items.Add(item);
                    }));

                }

                Utilities.Sleep(2);
            }
          
            foreach (ListViewItem Item in listView1.Items) {

                if (Item.Checked == true) {
                    string FullName = Item.Text;
                    try {

                        string Type = Item.SubItems[1].Text;
                        string Sig = Item.SubItems[2].Text;

                        string ProcName = System.IO.Path.GetFileNameWithoutExtension(FullName);
                        KillProcByName(ProcName);
                        label1.Text = $"Solving security flaw... {ProcName} - ({Sig})";
                        if (System.IO.File.Exists(FullName) == true) { System.IO.File.Delete(FullName); }

                        Item.BackColor = Color.Lime;

                        ThreadsCount++;
                        Utilities.Sleep(500, Utilities.Measure.Milliseconds);
                    } catch { if (Core.Settings.Experimental) new FileDeleter().Delete(FullName); Item.BackColor = Color.Red; }

                }

            }

            switch (CustomScanType)
            {
                case ScanType.Full:

                    break;
                case ScanType.Smart:

                    break;
                case ScanType.Custom:

                    break;
                case ScanType.USB:

                    try {
                        
                        string UnhideFiles_External = Utilities.RunConsole("attrib.exe", $"-h -r -s /s /d {CustomScanPath}*");

                        string AutorunUSB = System.IO.Path.Combine(CustomScanPath, "autorun.inf");

                        if (System.IO.File.Exists(AutorunUSB) == true) { System.IO.File.Delete(AutorunUSB); }

                        string desktopUSB = System.IO.Path.Combine(CustomScanPath, "desktop.inf");

                        if (System.IO.File.Exists(desktopUSB) == true) { System.IO.File.Delete(desktopUSB); }
                    
                    }  catch { }

                    try {

                        IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: CustomScanPath, searchOption: SearchOption.TopDirectoryOnly);

                        foreach (string File in Files)
                        {
                            try {
                                FileInfo FileInfo = new FileInfo(File);
                                if (FileInfo.Attributes != System.IO.FileAttributes.Normal) { FileInfo.Attributes = System.IO.FileAttributes.Normal; }

                            }
                            catch { }
                        }

                        List<string> SubDirs = FileDirSearcher.GetDirPaths(CustomScanPath, SearchOption.TopDirectoryOnly).ToList();

                        foreach (string Path in SubDirs)
                        {
                           
                            try
                            {
                                DirectoryInfo DirectoryInfo = new DirectoryInfo(Path);

                                if (string.Equals(DirectoryInfo.Name, "System Volume Information", StringComparison.OrdinalIgnoreCase) == true) { continue; }

                                if (DirectoryInfo.Attributes != System.IO.FileAttributes.Normal) { DirectoryInfo.Attributes = System.IO.FileAttributes.Normal; }

                                if (DirectoryInfo.Name == "")
                                {
                                    foreach (FileInfo subFiles in DirectoryInfo.EnumerateFiles())
                                    {
                                        try {
                                            subFiles.MoveTo(CustomScanPath);
                                        } catch { }
                                    }
                                    foreach (DirectoryInfo subFolder in DirectoryInfo.EnumerateDirectories())
                                    {
                                        try
                                        {
                                            subFolder.MoveTo(CustomScanPath);
                                        }
                                        catch { }
                                    }
                                    DirectoryInfo.Delete(true);
                                }
                            }
                            catch { }
                            
                        }

                    } catch { }

                    break;
                default:

                    break;
            }


            Parent.ListDrivers();
            //Parent.DetectThreads();
            Parent.ShowMessage($"{ThreadsCount} threats have been removed from your system. {Environment.NewLine + RepairFix}");
            Parent.CloseScanner(this, this.Parent);

        }

        private bool KillProcByName(string ProcName) {
            try {
                Process[] ProcList = Process.GetProcessesByName(ProcName);
                if (ProcList.Count() != 0)
                {
                    foreach (Process Proc in ProcList)
                    {
                        Proc.Kill();
                    }
                }
                return true;
            } catch { return false; }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Stop = true;
            Antivirus Parent = this.ParentForm as Antivirus;
            Parent.CloseScanner(this, this.Parent);
        }
        private void guna2Button1_CheckedChanged(object sender, EventArgs e)
        {
           
        }


        #endregion

        #region " AV "

        private bool USBScan(string PathEx)
        {

            bool ScanUSB = RecursiveDirScanner(PathEx, true);

            ListFiles(new List<AV_File>(), true);
            return true;
        }

        private void FullScan()
        {

            List<AV_File> RiskFiles = new List<AV_File>();

            List<ScanAction> StartupActionList = StartupList.Scan();

            this.Invoke(new Action(() =>
            {
                guna2ProgressBar1.Maximum = StartupActionList.Count;

            }));

            foreach (ScanAction action in StartupActionList)
            {

                try
                {

                    if (Stop == true) { ListFiles(RiskFiles); return; }

                    string FileName = action.Object;

                    if (System.IO.File.Exists(FileName) == false) { continue; }

                    this.Invoke(new Action(() =>
                    {
                        guna2ProgressBar1.Value += 1;
                        label1.Text = FileName;
                    }));


                    bool IsSystemFile = Utilities.IsSystem(FileName);

                    if (IsSystemFile == false && Utilities.IsScriptFormat(FileName) == true)
                    {
                        string GenMWName = XylonV2.CARO.VirNames.Generate(XylonV2.CARO.VirNames.Type.Behavior,
                                XylonV2.CARO.VirNames.Platforms.Win32, XylonV2.CARO.VirNames.Family.Inde,
                                XylonV2.CARO.VirNames.VariantL.D, XylonV2.CARO.VirNames.Suffixes.gen).ToString();
                        RiskFiles.Add(new AV_File(FileName, GenMWName)); continue;
                    }

                    if (IsSystemFile == false)
                    {
                        if (System.IO.File.Exists( SystemPaths.DefenderExeLocation) == true)
                        {

                            WindowsDefenderScanner scanner = new WindowsDefenderScanner(SystemPaths.DefenderExeLocation);
                            XylonV2.Engine.External.Core.ScanResult result = scanner.Scan(FileName);

                            if (result == XylonV2.Engine.External.Core.ScanResult.ThreatFound)
                            {
                                RiskFiles.Add(new AV_File(FileName, scanner.ResultParsed)); continue;
                            }

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

                    if (IsSystemFile == false && XylonV2.Engine.PE.Binary.PEChecker.IsNetAssembly(FileName) == true)
                    {
                        XylonV2.Engine.External.Core.DetectionResult ScanResult = XylonV2.Engine.PE.Net.Core.NetAnalysis.NetScanner(FileName);

                        if (ScanResult.Result == XylonV2.Engine.External.Core.ScanResult.ThreatFound)
                        {
                            RiskFiles.Add(new AV_File(FileName, ScanResult.Signature)); continue;
                        }
                    }

                }
                catch { }

            }

            ListFiles(RiskFiles, false);

            this.Invoke(new Action(() =>
            {
                label1.Text = "...";
                guna2ProgressBar1.Value = 0;
                guna2ProgressBar1.Maximum = 0;
            }));

            //bool ScanEx = RecursiveDirScanner(SystemPaths.systemDrive);
            bool ScanEx5 = RecursiveDirScanner(SystemPaths.Downloads);
            bool ScanEx4 = RecursiveDirScanner(SystemPaths.Documents);
            bool ScanEx6 = RecursiveDirScanner(SystemPaths.Prefetch);
            bool ScanEx7 = RecursiveDirScanner(SystemPaths.Temp);
            bool ScanEx8 = RecursiveDirScanner(SystemPaths.WindowsTemp);
            bool ScanEx1 = RecursiveDirScanner(SystemPaths.Appdata);
            bool ScanEx2 = RecursiveDirScanner(SystemPaths.Appdata_LocalLow);
            bool ScanEx3 = RecursiveDirScanner(SystemPaths.Appdata_Local);
           
           
           

            ListFiles(new List<AV_File>(), true);

        }

        public bool RecursiveDirScanner(string Dir, bool AgresiveScript = false)
        {
            try
            {
                IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: Dir, searchOption: SearchOption.TopDirectoryOnly, null, fileExtPatterns: new string[]
      {
                "*.apk",  "*.app", "*.bat","*.bin", "*.cmd", "*.com",
                "*.cpl", "*.csh", "*.exe", "*.js","*.vbscript", "*.vbs",
                "*.wsf", "*.py", "*.ps1", "*.lnk", "*.reg", "*.rgs", "*.run",
                "*.wsh", "*.hta", "*.vbe", "*.scr", "*.chk"
      }, ignoreCase: true, throwOnError: false);

               

                this.Invoke(new Action(() =>
                {
                    guna2ProgressBar1.Maximum = guna2ProgressBar1.Maximum + Files.Count();
                    label2.Text = "Scaning... " + Dir;
                    if (label1.Text.Length <= 500) { label1.Text += "."; } else { label1.Text = "."; }
                }));

                foreach (string FileName in Files)
                {
                    this.Invoke(new Action(() =>
                    {
                        guna2ProgressBar1.Value += 1;
                        label1.Text = FileName;
                    }));

                    ScanFile(FileName, AgresiveScript);
                }

                List<string> SubDirs = FileDirSearcher.GetDirPaths(Dir, SearchOption.AllDirectories).ToList();
               
                foreach (string Path in SubDirs)
                {
                    if (Stop == true) { ListFiles(new List<AV_File>()); return false; }
                    bool ConScan =  RecursiveDirScanner(Path);
                }
            }
            catch 
            {
                
            }
            return true;
        }

        private void SmartScan() {

            List<AV_File> RiskFiles = new List<AV_File>();

            List<ScanAction> StartupActionList = StartupList.Scan();

            this.Invoke(new Action(() =>
            {
                guna2ProgressBar1.Maximum = StartupActionList.Count;

            }));

            foreach (ScanAction action in StartupActionList) {

                try {

                    if (Stop == true) { ListFiles(RiskFiles); return; }

                    string FileName = action.Object;

                    if (System.IO.File.Exists(FileName) == false) { continue; }
                    //if (IsValidSize(FileName) == false) { continue; }

                    this.Invoke(new Action(() =>
                    {
                        guna2ProgressBar1.Value += 1;
                        label1.Text = FileName;
                    }));


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


                } catch { }
                
            }

            ListFiles(RiskFiles);

        }

        private bool CustomScan(string PathEx)
        {

            List<AV_File> RiskFiles = new List<AV_File>();

            if (System.IO.Directory.Exists(PathEx) == false) { ListFiles(RiskFiles); return false; }

            IScan CustomList = new CustomScanPath(PathEx);
          
           

            List<ScanAction> CustomActionList = CustomList.Scan();

            this.Invoke(new Action(() =>
            {
                guna2ProgressBar1.Maximum = CustomActionList.Count;

            }));

            foreach (ScanAction action in CustomActionList)
            {

                try
                {
                    if (Stop == true) { ListFiles(RiskFiles); return false; }

                    string FileName = action.Object;

                    if (System.IO.File.Exists(FileName) == false) { continue; }
                    if (IsValidSize(FileName) == false) { continue; }

                    this.Invoke(new Action(() =>
                    {
                        guna2ProgressBar1.Value += 1;
                        label1.Text = FileName;
                        //Utilities.Sleep(1);
                    }));

                    bool IsSystemFile = Utilities.IsSystem(FileName);

                    if (IsSystemFile == false && Utilities.IsScriptFormat(FileName) == true)
                    {
                        string GenMWName = XylonV2.CARO.VirNames.Generate(XylonV2.CARO.VirNames.Type.Rogue,
                                XylonV2.CARO.VirNames.Platforms.Win32, XylonV2.CARO.VirNames.Family.Inde,
                                XylonV2.CARO.VirNames.VariantL.D, XylonV2.CARO.VirNames.Suffixes.gen).ToString();
                        AddFile(new AV_File(FileName, GenMWName)); continue;
                    }

                    if (IsSystemFile == false )
                    {
                        if (System.IO.File.Exists(SystemPaths.DefenderExeLocation) == true)
                        {


                            WindowsDefenderScanner scanner = new WindowsDefenderScanner(SystemPaths.DefenderExeLocation);
                            XylonV2.Engine.External.Core.ScanResult result = scanner.Scan(FileName);


                            if (result == XylonV2.Engine.External.Core.ScanResult.ThreatFound)
                            {
                                AddFile(new AV_File(FileName, scanner.ResultParsed)); continue;
                            }


                        }
                    }

                    if (IsSystemFile == false && XylonV2.Engine.PE.Binary.PEChecker.IsNetAssembly(FileName) == true)
                    {
                        XylonV2.Engine.External.Core.DetectionResult ScanResult = XylonV2.Engine.PE.Net.Core.NetAnalysis.NetScanner(FileName);

                        if (ScanResult.Result == XylonV2.Engine.External.Core.ScanResult.ThreatFound)
                        {
                            AddFile(new AV_File(FileName, ScanResult.Signature)); continue;
                        }
                    }

                    if (IsSystemFile == false && Utilities.IsPotencialRiskFormat(FileName) == true)
                    {
                        XylonV2.Engine.External.Core.DetectionResult ScanResult = XylonV2.Engine.PE.Analysis.StringScan(FileName);

                        if (ScanResult.Result == XylonV2.Engine.External.Core.ScanResult.ThreatFound)
                        {
                            AddFile(new AV_File(FileName, ScanResult.Signature)); continue;
                        }
                    }


                }
                catch { }
                
            }

            ListFiles(RiskFiles);
            return true;
        }

        private bool ScanFile(string FileName, bool Agresive )
        {

                try
                {
                    if (Stop == true) {  return false; }

                if (System.IO.File.Exists(FileName) == false) { return false; }

                if (IsValidSize(FileName) == false) { return true; }

                this.Invoke(new Action(() =>
                    {
                        guna2ProgressBar1.Value += 1;
                        label1.Text = FileName;
                        //Utilities.Sleep(1);
                    }));


                    bool IsSystemFile = Utilities.IsSystem(FileName);

              

                if (IsSystemFile == false)
                    {
                        if (System.IO.File.Exists(SystemPaths.DefenderExeLocation) == true)
                        {


                            WindowsDefenderScanner scanner = new WindowsDefenderScanner(SystemPaths.DefenderExeLocation);
                            XylonV2.Engine.External.Core.ScanResult result = scanner.Scan(FileName);


                            if (result == XylonV2.Engine.External.Core.ScanResult.ThreatFound)
                            {
                                AddFile(new AV_File(FileName, scanner.ResultParsed)); return true;
                            }


                        }
                    }

                if (IsSystemFile == false && Agresive == true && Utilities.IsScriptFormat(FileName) == true)
                {
                    string GenMWName = XylonV2.CARO.VirNames.Generate(XylonV2.CARO.VirNames.Type.Behavior,
                            XylonV2.CARO.VirNames.Platforms.Win32, XylonV2.CARO.VirNames.Family.Inde,
                            XylonV2.CARO.VirNames.VariantL.D, XylonV2.CARO.VirNames.Suffixes.gen).ToString();
                    AddFile(new AV_File(FileName, GenMWName)); return true;
                }

                if (IsSystemFile == false && XylonV2.Engine.PE.Binary.PEChecker.IsNetAssembly(FileName) == true)
                    {
                        XylonV2.Engine.External.Core.DetectionResult ScanResult = XylonV2.Engine.PE.Net.Core.NetAnalysis.NetScanner(FileName);

                        if (ScanResult.Result == XylonV2.Engine.External.Core.ScanResult.ThreatFound)
                        {
                            AddFile(new AV_File(FileName, ScanResult.Signature)); return true;
                        }
                    }

                    if (IsSystemFile == false && Utilities.IsPotencialRiskFormat(FileName) == true)
                    {
                        XylonV2.Engine.External.Core.DetectionResult ScanResult = XylonV2.Engine.PE.Analysis.StringScan(FileName);

                        if (ScanResult.Result == XylonV2.Engine.External.Core.ScanResult.ThreatFound)
                        {
                            AddFile(new AV_File(FileName, ScanResult.Signature)); return true;
                        }
                    }


                }
                catch { }

            return true;
        }

        private void ListFiles(List<AV_File> FileNames, bool Terminate = true) {
            try {

                this.Invoke(new Action(() =>
                {
                    if (Terminate == true) guna2ProgressBar1.Maximum = FileNames.Count;
                    foreach (AV_File file in FileNames)
                    {
                        ListViewItem item = new ListViewItem(file.FileName);
                        item.SubItems.Add("File");
                        item.SubItems.Add(file.Id);
                        item.Checked = true;

                        if (CustomScanType == ScanType.Smart && Utilities.IsScriptFormat(file.FileName)) { item.BackColor = Color.Red; }

                        listView1.Items.Add(item);
                        if (Terminate == true) guna2ProgressBar1.Value += 1;
                    }

                    if (Terminate == true) {
                        guna2ProgressBar1.Maximum = 100;
                        guna2ProgressBar1.Value = guna2ProgressBar1.Maximum;

                        if (listView1.Items.Count == 0)
                        {
                            Antivirus Parent = this.ParentForm as Antivirus;

                            Parent.ListDrivers();
                            Parent.ShowMessage($"Congratulations, no threats found!");
                            Parent.CloseScanner(this, this.Parent);

                        }
                        else {
                            guna2Button1.Checked = true;
                            guna2Button1.Text = "Solve";
                            label1.Text = "";
                            label2.Text = "Waiting for user decision...";
                        }
                    }
                   
                }));

            } catch { }
        }

        private void AddFile(AV_File file)
        {
            try
            {

                this.Invoke(new Action(() =>
                {
                    ListViewItem item = new ListViewItem(file.FileName);
                    item.SubItems.Add("File");
                    item.SubItems.Add(file.Id);
                    item.Checked = true;
                    listView1.Items.Add(item);
                    guna2ProgressBar1.Value += 1;
                }));

            }
            catch { }
        }




        #endregion

        public bool IsValidSize(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                long fileSizeInBytes = fileInfo.Length;
                long fileSizeInMB = fileSizeInBytes / (1024 * 1024); // Convertir a megabytes

                // is minor of 200 MB
                return fileSizeInMB < 200;
            }
            catch 
            {
                return false;
            }
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.DrawBackground();
                bool value = true;
                try
                {
                    value = Convert.ToBoolean(e.Header.Tag);
                }
                catch (Exception)
                {
                }
                CheckBoxRenderer.DrawCheckBox(e.Graphics,
                    new Point(e.Bounds.Left + 4, e.Bounds.Top + 4),
                    value ? System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal :
                    System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
                TextRenderer.DrawText(e.Graphics, "Infected Files", new Font(FontFamily.GenericSerif, 9, FontStyle.Bold), new Point(e.Bounds.Left + 20, e.Bounds.Top + 4), Color.Red);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
            {
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(this.listView1.Columns[e.Column].Tag);
                }
                catch (Exception)
                {
                }
                this.listView1.Columns[e.Column].Tag = !value;
                foreach (ListViewItem item in this.listView1.Items)
                    item.Checked = !value;

                this.listView1.Invalidate();
            }
        }
    }
}
