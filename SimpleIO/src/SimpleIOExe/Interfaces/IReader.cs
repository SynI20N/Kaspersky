using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIOExe.Interfaces
{
    internal interface IReader : IDisposable
    {
        public string? ReadLine();
        public bool CanRead { get; }
    }
}
