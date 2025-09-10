using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleIOExe.Interfaces;

namespace SimpleIOExe.Services
{
    internal class FileLineReader : IReader
    {
        private StreamReader _reader;

        public FileLineReader(string path)
        {
            _reader = new StreamReader(path);
        }

        public bool CanRead => _reader.Peek() >= 0;

        public void Dispose()
        {
            _reader.Dispose();
        }

        public string? ReadLine()
        {
            return _reader.ReadLine();
        }
    }
}
