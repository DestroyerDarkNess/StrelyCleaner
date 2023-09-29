using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using StrelyCleaner.Core;
using StrelyCleaner.Core.Interfaces;
using StrelyCleaner.Core.Tweats;
using System.Diagnostics;
using StrelyCleaner.GUI.SubForms;
using StrelyCleaner.Core.Optimizer;
using HydraHelper.Adders.Base;
using StrelyCleaner.Helpers;
using System.Drawing;

namespace StrelyCleaner
{
   

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectionEntryPoint : Attribute
    {
        public bool CreateThread { get; set; } = true;
        public string BuildTarget { get; set; } = ".dll";
        public bool MergeLibs { get; set; } = false;
        public bool ILoader { get; set; } = false;
        public string ProtectionRules { get; set; } = string.Empty;
        public string ILoaderProtectionRules { get; set; } = string.Empty;
        public string PreCompiler { get; set; } = string.Empty;
        public string CloneTo { get; set; } = string.Empty;
        public bool BasicILoaderProtection { get; set; } = false;
    }


    internal static class Program
    {
       
        public static StrelyCleaner.GUI.Loading SplashForm = null;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetProcessDPIAware();

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [InjectionEntryPoint(CreateThread = true, MergeLibs = true, BuildTarget = ".exe", CloneTo = "C:\\Windows\\notepad.exe")]
        [STAThread]
        static void Main()
        {
            //FreeConsole();
            Global_Instances.LoadSettings();

            ExceptionManager.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);

            bool IsAdmin = Helpers.Functions.IsAdmin();
           
            //XylonV2.Modules.Initialization();

            string[] CommandLineArgs = Environment.GetCommandLineArgs();

            Core.FastArgumentParser FastArgumentParser = new Core.FastArgumentParser();

            IArgument GameArg = FastArgumentParser.Add("Game").SetDescription("Game FileName");

            FastArgumentParser.Parse(CommandLineArgs);
           
            if (string.IsNullOrEmpty(GameArg.Value) == false && System.IO.File.Exists(GameArg.Value)) {

                if (IsAdmin == false) {
                    Helpers.Functions.OpenAsAdmin(Application.ExecutablePath, String.Join(" ", CommandLineArgs));
                    Environment.Exit(0);
                }

                Func<bool> GameDialogAlive = () => true;
                Thread tGameLauncher = new Thread(() =>
                {
                    SetProcessDPIAware();
                   
                    GameLauncher Game = new GameLauncher(GameArg.Value);
                    ITempMainForm TempGame = Game;  GameDialogAlive = TempGame.MaintainThread;
                    Application.Run(Game);

                });

                tGameLauncher.SetApartmentState(ApartmentState.STA);
                tGameLauncher.Start();

                while (GameDialogAlive()) { }
                Environment.Exit(0);
            }

            bool IsSilent = CommandLineArgs.ToList().Contains("-silent");


            bool IsRuning = Utilities.My_Application_Is_Already_Running();

            if (IsRuning == true && IsSilent == false) {
                try {
                    string CurrentName = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
                    Process[] ProcList = Process.GetProcessesByName(CurrentName);
                    int CurrentID = Process.GetCurrentProcess().Id;

                    foreach (Process Proc in ProcList)
                    {

                        if (Proc.Id != CurrentID) { SendMessageToExistingInstance(Proc.MainWindowHandle); }

                    }

                    //DialogResult Msg = MessageBox.Show("The application is now open. check your TaskBar in windows.", "StrelyCleanner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Utilities.Sleep(1, Utilities.Measure.Minutes);
                    Environment.Exit(0); 
                } catch { }
            }

            if (IsAdmin == false)
            {
                Helpers.Functions.OpenAsAdmin(Application.ExecutablePath, String.Join(" ", CommandLineArgs));
                Environment.Exit(0);
            }

            Thread SplashThread = new Thread(() =>
            {
                if (IsSilent == false && Program.SplashForm == null)
                {
                    Program.SplashForm = new StrelyCleaner.GUI.Loading();
                    Size ScreenSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    SplashForm.NewLocation = new Point(ScreenSize.Width - SplashForm.Width - 10, 10);
                    Application.Run(Program.SplashForm);
                }

            });

            SplashThread.SetApartmentState(ApartmentState.STA);
            SplashThread.Start();

            if (Core.Global_Instances.Lite == false)
            {
                bool Runtime = true;

                Label FrameRateControl = null;

                Thread t = new Thread(() =>
                {
                    try
                    {
                        Global_Instances.CreateInstances();
                        SetProcessDPIAware();

                        bool IsLoaded = true;
                        while (IsLoaded) { IsLoaded = (Global_Instances.MainUI == null); Application.DoEvents(); }

                        Core.Global_Instances.MainUI.LoadHidding = IsSilent;

                        FrameRateControl = Core.Global_Instances.MainUI.GetFPSLabel();

                        Application.Run(Core.Global_Instances.MainUI);
                    }
                    catch { }
                    Runtime = false;
                });

                t.SetApartmentState(ApartmentState.STA);
                t.Start();


                while (Runtime)
                {

                    try
                    {

                        if (Core.Global_Instances.MainUI != null && Core.Global_Instances.MainUI.WindowState == FormWindowState.Normal && Core.Global_Instances.MainUI.Visible == true && Core.Global_Instances.MainUI.IsUILoaded == true && Core.Global_Instances.MainUI.CurrentPageView != null)
                        {

                            Global_Instances.RenderUI.UIRenderData(Global_Instances.MainUI.CurrentPageView); // Update BackEnd App Data

                            // Invoke Current View Thread
                            Core.Global_Instances.MainUI.CurrentPageView.Invoke((MethodInvoker)(() =>
                            {
                                Global_Instances.RenderUI.Render(Global_Instances.MainUI.CurrentPageView); // Render FrontEnd App Data
                                                                                                           //Core.Global_Instances.MainUI.UpdateScroll(); // Update Main Scroll
                                if (FrameRateControl != null) { FrameRateControl.Text = ($"FPS: {Global_Instances.RenderUI.FPS:F2}"); } // Show App Frames Per Seconds.

                            }));

                        }

                        Global_Instances.RenderUI?.Global_App_Services?.Execute();

                        if (Settings.Experimental)
                        {

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            GC.Collect();

                        }

                    }
                    catch (Exception e)
                    {

                        if (e is System.ComponentModel.InvalidAsynchronousStateException) { Runtime = false; }

                    }

                }
            }
            else {

                Global_Instances.CreateInstances();
                SetProcessDPIAware();

                bool IsLoaded = true;
                while (IsLoaded) { IsLoaded = (Global_Instances.MainUI == null); Application.DoEvents(); }

                Core.Global_Instances.MainUI.LoadHidding = IsSilent;
                Core.Global_Instances.MainUI.GetFPSLabel().Visible = false;

                Application.Run(Core.Global_Instances.MainUI);

            }

            Process.GetCurrentProcess().Kill();

            Environment.Exit(0);

        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static void SendMessageToExistingInstance(IntPtr Handle)
        {
            
            if (Handle != IntPtr.Zero)
            {
                SetWindowState.SetWindowState(Handle, SetWindowState.WindowState.Normal);
                SetWindowState.SetWindowState(Handle, SetWindowState.WindowState.Show);
                ShowWindow(Handle, SW_RESTORE);
                SetForegroundWindow(Handle);
            }
        }

    }
}
