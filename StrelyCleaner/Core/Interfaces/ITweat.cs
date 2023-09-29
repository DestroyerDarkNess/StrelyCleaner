using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Interfaces
{
   
    public interface ITweat
    {
        string id { get; }

        string Description { get; }

        bool Get();

        void Optimize();

        void Disabled();
    }

}
