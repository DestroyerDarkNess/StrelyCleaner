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
    internal class Opera : ICleaner
    {
        public System.Drawing.Image icon => Resources.icons8_opera_67;
        public string id => "Opera";

        public string Path => System.IO.Path.Combine(SystemPaths.Appdata_Local, @"Opera Software\Opera Stable");

        public string Description => "Turn off Unnecessary Windows Defender features that affect performance.";

        List<ICleanerOption> ICleaner.GetOptions => Options;


        private List<ICleanerOption> Options = null;

        public Opera()
        {
            Options = GetOptionsList();
        }


        private List<ICleanerOption> GetOptionsList()
        {
            var Result = new List<ICleanerOption>();

            Result.Add(Cache());
            Result.Add(Cookie());
            Result.Add(History());
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
            @"Cache\Cache_Data"
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

                string BaseDir = System.IO.Path.Combine(SystemPaths.Appdata, @"Opera Software\Opera Stable");

                if (System.IO.Directory.Exists(BaseDir) == true)
                {
                    string[] SubDirs = new string[]
 {
            @"GPUCache",
            @"Code Cache",
            @"File System",
            @"Service Worker\CacheStorage"
 };
                    foreach (string Dir in SubDirs)
                    {

                        string ToBaseDir = System.IO.Path.Combine(BaseDir, Dir);

                        if (System.IO.Directory.Exists(ToBaseDir) == true)
                        {
                            List<string> Files = FileDirSearcher.GetFilePaths(ToBaseDir, SearchOption.AllDirectories).ToList();

                            AllFiles.AddRange(Files);
                        }

                    }
                }

                if (System.IO.Directory.Exists(BaseDir) == true)
                {

                    IEnumerable<FileInfo> JournalFiles = FileDirSearcher.GetFiles(dirPath: BaseDir,
                   searchOption: SearchOption.AllDirectories);


                    foreach (FileInfo JFile in JournalFiles)
                    {
                        if (JFile.Name.ToLower().EndsWith("journal") == true)
                        {
                            if (double.Parse(new InfoFile(JFile.FullName).FileSize_MB) <= 0) { AllFiles.Add(JFile.FullName); }
                        }
                    }

                }


                if (System.IO.Directory.Exists(BaseDir) == true)
                {

                    IEnumerable<string> LogFiles = FileDirSearcher.GetFilePaths(dirPath: BaseDir, searchOption: SearchOption.AllDirectories, fileNamePatterns: new string[] { "*" }, fileExtPatterns: new string[] { "*.log" , "*.tmp" }, ignoreCase: true, throwOnError: false);
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

            string BaseDir = System.IO.Path.Combine(SystemPaths.Appdata, @"Opera Software\Opera Stable");

            ICleanerOption Result = new ICleanerOption() { id = "Cookie", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {
                string[] Files = new string[]
     {
            @"Network\Cookies",
             @"Network\Cookies-journal"
     };

                List<string> AllFiles = new List<string>();

                foreach (string File in Files)
                {

                    string ToBaseFile = System.IO.Path.Combine(BaseDir, File);

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
            string BaseDir = System.IO.Path.Combine(SystemPaths.Appdata, @"Opera Software\Opera Stable");

            ICleanerOption Result = new ICleanerOption() { id = "History", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {
                string[] Files = new string[]
     {
            @"Visited Links",
             @"Network Action Predictor",
             @"Top Sites"
     };

                List<string> AllFiles = new List<string>();

                foreach (string File in Files)
                {

                    string ToBaseFile = System.IO.Path.Combine(BaseDir, File);

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

        #region " Bookmarks "

        private ICleanerOption Bookmarks()
        {
            string BaseDir = System.IO.Path.Combine(SystemPaths.Appdata, @"Opera Software\Opera Stable");

            ICleanerOption Result = new ICleanerOption() { id = "Bookmarks", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {
                string[] Files = new string[]
     {
            @"Bookmarks",
             @"Bookmarks.bak"
     };

                List<string> AllFiles = new List<string>();

                foreach (string File in Files)
                {

                    string ToBaseFile = System.IO.Path.Combine(BaseDir, File);

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
