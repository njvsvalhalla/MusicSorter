using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicSorterCore
{
    public interface IMusicSorter
    {
        void Sort(string path, string outputPath, string format, bool actuallyMove);
    }
}
