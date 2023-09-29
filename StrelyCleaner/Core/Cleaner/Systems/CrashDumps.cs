using StrelyCleaner.Core.Interfaces;
using StrelyCleaner.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XylonV2;

namespace StrelyCleaner.Core.Cleaner.Systems
{
    internal class CrashDumps : ICleaner
    {
        public System.Drawing.Image icon => Resources.icons8_windows_48;
        public string id => "CrashDumps";

        public string Path => System.IO.Path.Combine(SystemPaths.Appdata_Local, @"CrashDumps");

        public string Description => "Turn off Unnecessary Windows Defender features that affect performance.";

        List<ICleanerOption> ICleaner.GetOptions => Options;


        private List<ICleanerOption> Options = null;

        public CrashDumps()
        {
            Options = GetOptionsList();
        }


        private List<ICleanerOption> GetOptionsList()
        {
            var Result = new List<ICleanerOption>();

            Result.Add(Dumps());

            return Result;
        }

        #region " Dumps "

        private ICleanerOption Dumps()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Dumps", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {

                List<string> AllFiles = new List<string>();

                if (System.IO.Directory.Exists(Path) == true)
                {
                    IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: Path, searchOption: SearchOption.AllDirectories, 
                        fileNamePatterns: new string[] { "*" }, fileExtPatterns: new string[] { "*.dmp" }, ignoreCase: true, throwOnError: false);

                    AllFiles.AddRange(Files);
                }

                String ToBasePath = System.IO.Path.Combine(SystemPaths.Windows);
                if (System.IO.Directory.Exists(ToBasePath) == true)
                {
                    IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: ToBasePath, searchOption: SearchOption.AllDirectories,
                        fileNamePatterns: new string[] { "*" }, fileExtPatterns: new string[] { "*.dmp", "*.log" }, ignoreCase: true, throwOnError: false);

                    AllFiles.AddRange(Files);
                }


                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }


        #endregion

    }
}
