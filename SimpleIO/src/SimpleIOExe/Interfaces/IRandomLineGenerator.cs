using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleIOExe.Interfaces
{
    internal interface IRandomLineGenerator
    {
        public List<string> GetSurnames();
        public List<string> GetNames();

        public int GetRandomAge();

        public string GetRandomLine();
    }
}
