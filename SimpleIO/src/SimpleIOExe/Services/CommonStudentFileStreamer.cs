using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleIOExe.Interfaces;

namespace SimpleIOExe.Services
{
    internal class CommonStudentFileStreamer : StudentFileStreamer
    {
        public CommonStudentFileStreamer(IReader reader, IWriter writer) : base(reader, writer)
        {

        }

        public void StreamFile()
        {
            int count = 0;
            while(_reader.CanRead)
            {
                string? s = StreamLine(ref _reader, ref _writer);
                if (s == null)
                {
                    continue;
                }
                CheckAge(ref count, s);
            }
            _writer.WriteLine($"Людей возраста > 20: {count}");
        }

        private int CheckAge(ref int count, string? s)
        {
            string[] words = s.Split(' ');
            if (words.Length != 3) { throw new FormatException(); }
            if (Convert.ToInt32(words[2]) > 20)
            {
                count++;
            }

            return count;
        }
    }
}
