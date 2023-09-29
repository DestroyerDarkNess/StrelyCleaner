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
    internal class Temp : ICleaner
    {
        public System.Drawing.Image icon => Resources.icons8_windows_48;
        public string id => "Temp";

        public string Path => SystemPaths.Temp;

        public string Description => "Turn off Unnecessary Windows Defender features that affect performance.";

        List<ICleanerOption> ICleaner.GetOptions => Options;


        private List<ICleanerOption> Options = null;

        public Temp()
        {
            Options = GetOptionsList();
        }


        private List<ICleanerOption> GetOptionsList()
        {
            var Result = new List<ICleanerOption>();

            Result.Add(Files());

            return Result;
        }

        #region " Files "

        private ICleanerOption Files()
        {

            ICleanerOption Result = new ICleanerOption() { id = "Files", Parent = id, Enabled = false, Type = CleanOptionType.File };

            Func<List<string>> GetDataFunction = delegate ()
            {

                List<string> AllFiles = new List<string>();

                if (System.IO.Directory.Exists(Path) == true)
                {
                    List<string> Files = FileDirSearcher.GetFilePaths(Path, SearchOption.AllDirectories).ToList();

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
