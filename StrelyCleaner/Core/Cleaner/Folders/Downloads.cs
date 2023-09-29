using StrelyCleaner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.Design.AxImporter;
using XylonV2;
using StrelyCleaner.Properties;

namespace StrelyCleaner.Core.Cleaner.Folders
{
    internal class Downloads : ICleaner
    {
        public System.Drawing.Image icon => Resources.icons8_folder_48;
        public string id => "Downloads";

    public string Path => SystemPaths.Downloads;

    public string Description => "Turn off Unnecessary Windows Defender features that affect performance.";

    List<ICleanerOption> ICleaner.GetOptions => Options;


    private List<ICleanerOption> Options = null;

    public Downloads()
    {
        Options = GetOptionsList();
    }


    private List<ICleanerOption> GetOptionsList()
    {
        var Result = new List<ICleanerOption>();

        Result.Add(Programs());
        Result.Add(Compressed());
        Result.Add(Images());
        Result.Add(Music());
        Result.Add(Video());
        Result.Add(Documents());

            return Result;
    }

        #region " Programs "

        private ICleanerOption Programs()
    {

        ICleanerOption Result = new ICleanerOption() { id = "Programs", Parent = id, Enabled = false, Type = CleanOptionType.File };

        Func<List<string>> GetDataFunction = delegate ()
        {

            List<string> AllFiles = new List<string>();

            if (System.IO.Directory.Exists(Path) == true)
            {
                IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: Path, searchOption: SearchOption.AllDirectories,
                         fileNamePatterns: new string[] { "*" }, fileExtPatterns: new string[] { "*.exe", "*.msi", "*.com" }, ignoreCase: true, throwOnError: false);

                AllFiles.AddRange(Files);
            }


            return AllFiles;
        };

        Result.Data = GetDataFunction;

        return Result;
    }


        #endregion

        #region " Compressed "

        private ICleanerOption Compressed()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Compressed", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {

                List<string> AllFiles = new List<string>();

                if (System.IO.Directory.Exists(Path) == true)
                {
                    IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: Path, searchOption: SearchOption.AllDirectories,
                             fileNamePatterns: new string[] { "*" }, fileExtPatterns: new string[] { "*.rar", "*.zip", "*.7z", "*.gz", "*.cab", "*.bz2", "*.tar" }, ignoreCase: true, throwOnError: false);

                    AllFiles.AddRange(Files);
                }


                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }


        #endregion

        #region " Images "

        private ICleanerOption Images()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Images", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {

                List<string> AllFiles = new List<string>();

                if (System.IO.Directory.Exists(Path) == true)
                {
                    string[] ImagesFormat = new string[]
{
    ".jpg",
    ".jpeg",
    ".png",
    ".gif",
    ".bmp",
    ".tiff",
    ".svg",
    ".ico",
    ".jp2",
    ".webp",
    ".heif",
    ".pbm",
    ".pgm",
    ".ppm",
    ".exif",
};
                    IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: Path, searchOption: SearchOption.AllDirectories,
                             fileNamePatterns: new string[] { "*" }, fileExtPatterns: ImagesFormat, ignoreCase: true, throwOnError: false);

                    AllFiles.AddRange(Files);
                }


                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }


        #endregion

        #region " Video "

        private ICleanerOption Video()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Video", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {

                List<string> AllFiles = new List<string>();

                if (System.IO.Directory.Exists(Path) == true)
                {
                    string[] Format = new string[]
  {
    ".mp4",
    ".avi",
    ".mkv",
    ".wmv",
    ".mov",
    ".flv",
    ".webm",
    ".m4v",
    ".3gp",
    ".mpeg",
    ".mpg",
    ".vob",
    ".rm",
    ".rmvb",
  };

                    IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: Path, searchOption: SearchOption.AllDirectories,
                             fileNamePatterns: new string[] { "*" }, fileExtPatterns: Format, ignoreCase: true, throwOnError: false);

                    AllFiles.AddRange(Files);
                }


                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }


        #endregion

        #region " Music "

        private ICleanerOption Music()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Music", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {

                List<string> AllFiles = new List<string>();

                if (System.IO.Directory.Exists(Path) == true)
                {
                    string[] Formats = new string[]
{
    ".mp3",
    ".wav",
    ".flac",
    ".aac",
    ".wma",
    ".ogg",
    ".m4a",
    ".ac3",
    ".ape",
    ".alac",
};
                    IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: Path, searchOption: SearchOption.AllDirectories,
                             fileNamePatterns: new string[] { "*" }, fileExtPatterns: Formats, ignoreCase: true, throwOnError: false);

                    AllFiles.AddRange(Files);
                }


                return AllFiles;
            };

            Result.Data = GetDataFunction;

            return Result;
        }


        #endregion

        #region " Documents "

        private ICleanerOption Documents()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Documents", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {

                List<string> AllFiles = new List<string>();

                if (System.IO.Directory.Exists(Path) == true)
                {
                    string[] Formats = new string[]
 {
    ".pdf",
    ".doc",
    ".docx",
    ".xls",
    ".xlsx",
    ".ppt",
    ".pptx",
    ".txt",
    ".rtf",
    ".html",
    ".csv",
    ".xml",
    ".json",
    ".odt",
    ".ods",
    ".odp",
 };

                    IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: Path, searchOption: SearchOption.AllDirectories,
                             fileNamePatterns: new string[] { "*" }, fileExtPatterns: Formats, ignoreCase: true, throwOnError: false);

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
