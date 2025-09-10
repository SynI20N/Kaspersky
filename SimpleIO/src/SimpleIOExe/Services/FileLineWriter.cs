using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleIOExe.Interfaces;

namespace SimpleIOExe.Services
{
    internal class FileLineWriter : IWriter
    {
        private StreamWriter _reader;

        public FileLineWriter(string path, bool append)
        {
            _reader = new StreamWriter(path, append);
        }
        public void Dispose()
        {
            _reader.Dispose();
        }

        public void WriteLine(string data)
        {
            _reader.WriteLine(data);
        }
    }
}
