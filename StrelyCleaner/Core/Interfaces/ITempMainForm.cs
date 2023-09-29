using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Interfaces
{
    public interface ITempMainForm
    {
        Func<bool> MaintainThread { get; }
    }
}
