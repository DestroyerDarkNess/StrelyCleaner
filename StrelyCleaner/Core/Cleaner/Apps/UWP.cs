using StrelyCleaner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XylonV2.Core.File;
using XylonV2;
using StrelyCleaner.Properties;

namespace StrelyCleaner.Core.Cleaner.Apps
{
    internal class UWP : ICleaner
    {
        public System.Drawing.Image icon => Resources.icons8_apps_tab_48;

        public string id => "UWP_Apps";

        public string Path => System.IO.Path.Combine(SystemPaths.Appdata_Local, @"Packages");

        public string Description => "Turn off Unnecessary Windows Defender features that affect performance.";

        List<ICleanerOption> ICleaner.GetOptions => Options;


        private List<ICleanerOption> Options = null;

        public UWP()
        {
            Options = GetOptionsList();
        }


        private List<ICleanerOption> GetOptionsList()
        {
            var Result = new List<ICleanerOption>();

            Result.Add(Cache());

            return Result;
        }

        #region " Cache "

        private ICleanerOption Cache()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Cache and Logs", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {

                List<string> AllFiles = new List<string>();

                if (System.IO.Directory.Exists(Path) == true) {
                    IEnumerable<string> Dirs = FileDirSearcher.GetDirPaths(dirPath: Path, searchOption: SearchOption.AllDirectories, dirPathPatterns: new string[]
      {
                "*"
      }, dirNamePatterns: new string[]
              {
                "*Cache_Data*"
      }, ignoreCase: true, throwOnError: false);


                    foreach (string Dir in Dirs)
                    {

                        //string ToBaseDir = System.IO.Path.Combine(Path, Dir);

                        if (System.IO.Directory.Exists(Dir) == true)
                        {
                            List<string> Files = FileDirSearcher.GetFilePaths(Dir, SearchOption.AllDirectories).ToList();

                            AllFiles.AddRange(Files);
                        }

                    }

                    IEnumerable<string> LogFiles = FileDirSearcher.GetFilePaths(dirPath: Path, searchOption: SearchOption.AllDirectories, fileNamePatterns: new string[] { "*" }, fileExtPatterns: new string[] { "*.log", "*.tmp" }, ignoreCase: true, throwOnError: false);
                    AllFiles.AddRange(LogFiles);
                }

                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }


        #endregion


    }
}
