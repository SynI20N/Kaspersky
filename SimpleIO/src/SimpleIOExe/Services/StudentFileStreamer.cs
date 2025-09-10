using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleIOExe.Interfaces;

namespace SimpleIOExe.Services
{
    internal class StudentFileStreamer : IFileStreamer, IDisposable
    {
        protected IReader _reader;
        protected IWriter _writer;

        public StudentFileStreamer(IReader reader, IWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public void Dispose()
        {
            _reader.Dispose();
            _writer.Dispose();
        }

        public string? StreamLine(ref IReader from, ref IWriter to)
        {
            string? s = _reader.ReadLine();
            if(s == null)
            {
                return null;
            }
            _writer.WriteLine(s);
            return s;
        }
    }
}
