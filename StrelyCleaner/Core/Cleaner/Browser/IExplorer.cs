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

namespace StrelyCleaner.Core.Cleaner.Browser
{
    internal class IExplorer : ICleaner
    {
        public System.Drawing.Image icon => Resources.icons8_internet_explorer_48;
        public string id => "IExplorer";

        public string Path => System.IO.Path.Combine(SystemPaths.Appdata_Local, @"Microsoft");

        public string Description => "Turn off Unnecessary Windows Defender features that affect performance.";

        List<ICleanerOption> ICleaner.GetOptions => Options;


        private List<ICleanerOption> Options = null;

        public IExplorer()
        {
            Options = GetOptionsList();
        }


        private List<ICleanerOption> GetOptionsList()
        {
            var Result = new List<ICleanerOption>();

            Result.Add(Cache());
            Result.Add(Cookie());

            return Result;
        }

        #region " Cache "

        private ICleanerOption Cache()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Cache", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {
                string[] Dirs = new string[]
     {
            @"Windows\INetCache\IE"
     };

                List<string> AllFiles = new List<string>();

                foreach (string Dir in Dirs)
                {

                    string ToBaseDir = System.IO.Path.Combine(Path, Dir);

                    if (System.IO.Directory.Exists(ToBaseDir) == true)
                    {
                        List<string> Files = FileDirSearcher.GetFilePaths(ToBaseDir, SearchOption.AllDirectories).ToList();

                        AllFiles.AddRange(Files);
                    }

                }


                string[] FilesEx = new string[]
    {
            @"Internet Explorer\brndlog.bak",
             @"Internet Explorer\brndlog.txt"
    };

                foreach (string File in FilesEx)
                {

                    string ToBaseFile = System.IO.Path.Combine(Path, File);

                    if (System.IO.File.Exists(ToBaseFile) == true)
                    {
                        AllFiles.Add(ToBaseFile);
                    }

                }

                string BasePath = System.IO.Path.Combine(Path, @"Internet Explorer");
                if (System.IO.Directory.Exists(BasePath) == true)
                {

                    IEnumerable<string> LogFiles = FileDirSearcher.GetFilePaths(dirPath: BasePath, searchOption: SearchOption.AllDirectories, fileNamePatterns: new string[] { "*" }, fileExtPatterns: new string[] { "*.log" }, ignoreCase: true, throwOnError: false);
                    AllFiles.AddRange(LogFiles);

                }

                AllFiles.RemoveAll(nombreArchivo => System.IO.Path.GetFileName( nombreArchivo).Equals("container.dat", StringComparison.OrdinalIgnoreCase));

                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }


        #endregion



        #region " Cookie "

        private ICleanerOption Cookie()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Cookie", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {
                string[] Dirs = new string[]
     {
            @"Internet Explorer\DOMStore"
     };

                List<string> AllFiles = new List<string>();

                foreach (string Dir in Dirs)
                {

                    string ToBaseDir = System.IO.Path.Combine(Path, Dir);

                    if (System.IO.Directory.Exists(ToBaseDir) == true)
                    {
                        List<string> Files = FileDirSearcher.GetFilePaths(ToBaseDir, SearchOption.AllDirectories).ToList();

                        AllFiles.AddRange(Files);
                    }

                }

                AllFiles.RemoveAll(nombreArchivo => System.IO.Path.GetFileName(nombreArchivo).Equals("container.dat", StringComparison.OrdinalIgnoreCase));

                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }

        #endregion







    }
}
