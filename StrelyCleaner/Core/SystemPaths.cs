using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core
{
    internal class SystemPaths
    {
        public static readonly string DefenderExeLocation = @"C:\Program Files\Windows Defender\MpCmdRun.exe";
        public static readonly string systemDrive = Path.GetPathRoot(Environment.SystemDirectory);
        public static readonly string Windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        public static readonly string WindowsTemp = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\Temp";
        public static readonly string Temp = Path.GetTempPath();

        public static readonly string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string Appdata_Local = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Appdata\local";
        public static readonly string Appdata_LocalLow = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Appdata\LocalLow";

        public static readonly string Recent = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
        public static readonly string Prefetch = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer) + @"\Windows\Prefetch";

        public static readonly string Downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
        public static readonly string Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static readonly string Disk = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

        public static readonly string ProgramFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        public static readonly string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

    }
}
