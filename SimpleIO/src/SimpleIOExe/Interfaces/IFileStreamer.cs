using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIOExe.Interfaces
{
    internal interface IFileStreamer
    {
        public string? StreamLine(ref IReader from, ref IWriter to);
    }
}
