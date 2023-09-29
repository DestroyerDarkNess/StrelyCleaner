using StrelyCleaner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using XylonV2;

namespace StrelyCleaner.Core.Antivirus
{
    public class CustomScanPath : IScan
    {
        public string id => "CustomScan";

        public string Description => "Search Files and Folders to Scan it.";

        private string Path = string.Empty;

        public CustomScanPath(string Dir) { Path = Dir; }

        public List<ScanAction> Scan()
        {
            List<ScanAction> Result = new List<ScanAction>();

            List<string> Files = FileDirSearcher.GetFilePaths(Path, SearchOption.AllDirectories).ToList();

            foreach (string FileName in Files)
            {
                if (Utilities.IsPotencialRiskFormat(FileName) == true) {

                    ScanAction ScanObject = new ScanAction();
                    ScanObject.Object = FileName;
                    ScanObject.Type = ScanOptionType.File;
                    Result.Add(ScanObject);

                } 
            }

            return Result;
        }
    }
}
