using StrelyCleaner.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrelyCleaner.External._3D_Analyze
{
    public interface IOption
    {
        string id { get; }

        string Name { get; }

        string Description { get; }

        bool Enabled { get; set; }

        int Value { get; set; }
    }

    public interface IRenderOption
    {
        string id { get; }

        List<IOption> Options { get; }

    }

}
