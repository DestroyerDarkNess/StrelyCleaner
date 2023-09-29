using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.External._3D_Analyze
{
    internal class DX : IRenderOption
    {
        public string id => "DX9";

        private List<IOption> OptionList;
        public List<IOption> Options => OptionList;

        public DX() {
            List<IOption> OptionEx = new List<IOption>();


            OptionList = OptionEx;
        }




    }
}
