using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleIOExe.Interfaces;

namespace SimpleIOExe.Services
{
    internal class ConsoleStreamer : IReader, IWriter
    {
        public ConsoleStreamer()
        {

        }

        public bool CanRead => true;

        public void Dispose()
        {
            
        }

        public string? ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string data)
        {
            Console.WriteLine(data);
        }
    }
}
