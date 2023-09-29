using StrelyCleaner.Core.Interfaces;
using StrelyCleaner.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XylonV2;
using XylonV2.Core.File;
using static System.Net.WebRequestMethods;

namespace StrelyCleaner.Core.Cleaner.Browser
{
    internal class Edge : ICleaner
    {
        public System.Drawing.Image icon => Resources.icons8_edge_48;
        public string id => "Edge";

        public string Path => System.IO.Path.Combine(SystemPaths.Appdata_Local, @"Microsoft\Edge");

        public string Description => "Turn off Unnecessary Windows Defender features that affect performance.";

        List<ICleanerOption> ICleaner.GetOptions => Options;


        private List<ICleanerOption> Options = null;

        public Edge() {
            Options = GetOptionsList();
        }


        private List<ICleanerOption> GetOptionsList()
        {
         var Result = new List<ICleanerOption>();

            Result.Add(Cache());
            Result.Add(Cookie());
            Result.Add(History());
            Result.Add(Metrics());
            Result.Add(Bookmarks());

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
            @"User Data\Default\Cache",
            @"User Data\Default\Code Cache",
            @"User Data\Default\Service Worker\CacheStorage"
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

                string JournalPath = System.IO.Path.Combine(Path, @"User Data\Default");
                if (System.IO.Directory.Exists(JournalPath) == true)
                {

                    IEnumerable<FileInfo> JournalFiles = FileDirSearcher.GetFiles(dirPath: JournalPath,
                   searchOption: SearchOption.AllDirectories);


                    foreach (FileInfo JFile in JournalFiles)
                    {
                        if (JFile.Name.ToLower().EndsWith("journal") == true)
                        {
                            if (double.Parse(new InfoFile(JFile.FullName).FileSize_MB) <= 0) { AllFiles.Add(JFile.FullName); }
                        }
                    }

                }

                string BasePath = System.IO.Path.Combine(Path, @"User Data\Default");
                if (System.IO.Directory.Exists(BasePath) == true)
                {

                    IEnumerable<string> LogFiles = FileDirSearcher.GetFilePaths(dirPath: BasePath, searchOption: SearchOption.AllDirectories, fileNamePatterns: new string[] {"*" }, fileExtPatterns: new string[]   { "*.log" }, ignoreCase: true, throwOnError: false);
                    AllFiles.AddRange(LogFiles);

                }


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
                string[] Files = new string[]
     {
            @"User Data\Default\Network\Cookies",
             @"User Data\Default\Network\Cookies-journal"
     };

                List<string> AllFiles = new List<string>();

                foreach (string File in Files)
                {

                    string ToBaseFile = System.IO.Path.Combine(Path, File);

                    if (System.IO.File.Exists(ToBaseFile) == true)
                    {
                        AllFiles.Add(ToBaseFile);
                    }

                }

                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }

        #endregion

        #region " History "

        private ICleanerOption History()
        {

            ICleanerOption Result = new ICleanerOption() { id = "History", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {
                string[] Files = new string[]
     {
            @"User Data\Default\Visited Links",
             @"User Data\Default\Network Action Predictor",
             @"User Data\Default\Top Sites"
     };

                List<string> AllFiles = new List<string>();

                foreach (string File in Files)
                {

                    string ToBaseFile = System.IO.Path.Combine(Path, File);

                    if (System.IO.File.Exists(ToBaseFile) == true)
                    {
                        AllFiles.Add(ToBaseFile);
                    }

                }

                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }

        #endregion

        #region " Metrics "

        private ICleanerOption Metrics()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Metrics", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {
                string[] Dirs = new string[]
     {
            @"User Data\BrowserMetrics"
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

                string BasePath = System.IO.Path.Combine(Path, @"User Data\CrashpadMetrics-active.pma");
                if (System.IO.File.Exists(BasePath) == true)
                {
                    AllFiles.Add(BasePath);
                }


                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }


        #endregion

        #region " Bookmarks "

        private ICleanerOption Bookmarks()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Bookmarks", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {
                string[] Files = new string[]
     {
            @"User Data\Default\Bookmarks",
             @"User Data\Default\Bookmarks.bak"
     };

                List<string> AllFiles = new List<string>();

                foreach (string File in Files)
                {

                    string ToBaseFile = System.IO.Path.Combine(Path, File);

                    if (System.IO.File.Exists(ToBaseFile) == true)
                    {
                        AllFiles.Add(ToBaseFile);
                    }

                }

                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }

        #endregion


    }
}
