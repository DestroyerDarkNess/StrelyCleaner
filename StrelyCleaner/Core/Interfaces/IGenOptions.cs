using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.Core.Interfaces
{
    public interface IGenOptions
    {
        string id { get; }

        string Description { get; }

        void Execute();
    }
}
