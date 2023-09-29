using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Interfaces
{

    public enum CleanOptionType
    {
        File = 0,
        Directory = 1,
        Registry = 2,
        Configuration = 3
    }

    public class ICleanerOption
    {
        public string id { get; set; }
        public string Parent { get; set; }
        public bool Enabled { get; set; }
        public CleanOptionType Type { get; set; }
        public Func<List<string>> Data { get; set; }
    }
    public interface ICleaner
    {
        Image icon { get; }

        string id { get; }

        string Path { get; }

        string Description { get; }

        List<ICleanerOption> GetOptions { get; }

    }
}
