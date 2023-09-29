using IWshRuntimeLibrary;
using StrelyCleaner.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrelyCleaner.Core.Optimizer
{
    public class Game { 
    
        public int processId { get; set; }

        public Game(int ID) { processId = ID; }

        public  bool IsRunning()
        {
            try
            {
                Process.GetProcessById(processId);
                return true;
            }
            catch (ArgumentException)
            {
                return false; 
            }
        }

    }


    public class SentinelGameLauncher
    {
        public string GamePath { get; set; }
        public string GameFolder { get; set; }
        public string CommandLineOptions { get; set; }

        public bool FakeFullScreen = false;

        public bool WindowMode = false;

        public SentinelGameLauncher(string gamePath)
        {
            GamePath = gamePath;
            GameFolder = System.IO.Path.GetDirectoryName(gamePath);
        }

        public Game Launch()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = GamePath,
                    UseShellExecute = true,
                    CreateNoWindow = false,
                    WorkingDirectory = GameFolder,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    WindowStyle = ProcessWindowStyle.Normal, 
                    Arguments = CommandLineOptions 
                };

                Process gameProcess = new Process { StartInfo = startInfo };

                if (gameProcess.Start())
                {
                  gameProcess.WaitForInputIdle();

                  Game NewGame = new Game(gameProcess.Id);
                  while (gameProcess.MainWindowHandle == IntPtr.Zero) { }

                    if (WindowMode == true)
                    {

                     if (IsWindowMaximized(gameProcess.MainWindowHandle) == true)  ToggleFullscreen(gameProcess.MainWindowHandle);

                        for (int i = 0; i < 10; i++) { 

                            bool IsMaximized = IsWindowMaximized(gameProcess.MainWindowHandle);

                        if (IsMaximized == true)
                        {

                            //MoveWindow(gameProcess.MainWindowHandle, 5, 5, 600, 800, true);
                            PostMessage(gameProcess.MainWindowHandle, WM_MOVE, IntPtr.Zero, IntPtr.Zero);
                            ForceWindowMode(gameProcess.MainWindowHandle);
                            SetWindowStyle.SetWindowStyle(gameProcess.MainWindowHandle, SetWindowStyle.WindowStyles.WS_BORDER);
                            SetWindowState.SetWindowState(gameProcess.MainWindowHandle, SetWindowState.WindowState.Normal);
                         }
                        else
                        {
                                break;
                        }
                       }

                        
                    }

                    if (FakeFullScreen == true) {
                       

                       for (int i = 0; i < 3; i++)
                        {
                            if (NewGame.IsRunning() == false) { break; } 


                            var placement = GetPlacement(gameProcess.MainWindowHandle);

                                if (string.Equals(placement.showCmd.ToString(), "maximized", StringComparison.OrdinalIgnoreCase) == false)
                                {
                                    bool FakeFullSc = FullScreenEmulation(gameProcess);
                                    i -= 1;
                                }
                                else { break; }
                        }
                    }


                    return NewGame;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

       
        public void Close()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("game");

                foreach (Process process in processes)
                {
                    process.Kill();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                // Manejo de error: se produjo una excepción al cerrar el juego.
                Console.WriteLine("Error al cerrar el juego: " + ex.Message);
            }
        }

        public void CreateShortcutOnDesktop(string shortcutName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string shortcutPath = Path.Combine(desktopPath, $"{shortcutName}.lnk");

            if (!System.IO.File.Exists(shortcutPath))
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = Application.ExecutablePath; // GamePath;
                shortcut.Arguments = "-Game " + GamePath;
                shortcut.IconLocation = GamePath;
                shortcut.Save();
            }
            else
            {
                Console.WriteLine("El acceso directo ya existe en el escritorio.");
            }
        }

        public bool IsShortcutOnDesktop(string shortcutName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string shortcutPath = Path.Combine(desktopPath, $"{shortcutName}.lnk");

            return System.IO.File.Exists(shortcutPath);
        }

        public string GetShortcutPath(string shortcutName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string shortcutPath = Path.Combine(desktopPath, $"{shortcutName}.lnk");

            return shortcutPath;
        }


        #region " Fake FullScreen "

        public bool FullScreenEmulation(Process Process)
        {
            try
            {

                IntPtr HWND = Process.MainWindowHandle;
                for (int i = 0; i <= 2; i++)
                {
                    SetWindowStyle.SetWindowStyle(HWND, SetWindowStyle.WindowStyles.WS_BORDER);
                    SetWindowState.SetWindowState(HWND, SetWindowState.WindowState.Maximize);
                }
                BringMainWindowToFront(Process);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        private bool FisrsFocus = false;

        public void BringMainWindowToFront(Process bProcess)
        {
            if (FisrsFocus == false)
            {
                if (bProcess != null)
                    SetForegroundWindow(bProcess.MainWindowHandle);
                FisrsFocus = true;
            }
        }



        private static WINDOWPLACEMENT GetPlacement(IntPtr hwnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return placement;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public ShowWindowCommands showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1,
            Minimized = 2,
            Maximized = 3
        }

        private const int SW_RESTORE = 9;
        private const UInt32 WM_MOVE = 0x0003;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
      
        public static void ForceWindowMode(IntPtr hWnd)
        {
            ShowWindow(hWnd, SW_RESTORE);
        }

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private const uint WM_KEYDOWN = 0x0100;
        private const int VK_RETURN = 0x0D;
        private const int VK_MENU = 0x12; // Código de tecla Alt

        public static void ToggleFullscreen(IntPtr hWnd)
        {
            // Simula la pulsación de la tecla Alt
            PostMessage(hWnd, WM_KEYDOWN, (IntPtr)VK_MENU, IntPtr.Zero);
            // Simula la pulsación de la tecla Enter
            PostMessage(hWnd, WM_KEYDOWN, (IntPtr)VK_RETURN, IntPtr.Zero);
            // Libera la tecla Alt (para evitar que quede presionada)
            PostMessage(hWnd, WM_KEYDOWN, (IntPtr)VK_MENU, IntPtr.Zero);
        }

        public static bool IsWindowMaximized(IntPtr hWnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT
            {
                length = Marshal.SizeOf(typeof(WINDOWPLACEMENT))
            };

            if (GetWindowPlacement(hWnd, ref placement))
            {
                return placement.showCmd == ShowWindowCommands.Maximized;
            }

            return false; // No se pudo obtener la información de la ventana
        }


        #endregion


    }
}
