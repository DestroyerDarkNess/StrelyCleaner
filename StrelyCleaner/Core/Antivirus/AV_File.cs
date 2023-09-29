using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Antivirus
{
    public class AV_File
    {

        public string Id = string.Empty; 
        public string FileName = string.Empty;

        public AV_File(string File, string Vir = "") { FileName = File; Id = Vir; }

    }
}
