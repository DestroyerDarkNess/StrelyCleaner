using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Interfaces
{

    public enum ScanOptionType
    {
        File = 0,
        Directory = 1,
        Registry = 2,
        Configuration = 3
    }

    public class ScanAction {

        public string Object;
        public ScanOptionType Type;
    
    }


    public interface IScan
    {
        string id { get; }

        string Description { get; }

        List<ScanAction> Scan();

    }
}
