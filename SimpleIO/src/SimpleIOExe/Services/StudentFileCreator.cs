using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleIOExe.Interfaces;

namespace SimpleIOExe.Services
{
    internal class StudentFileCreator : IFileCreator, IDisposable
    {
        private IWriter _stream;
        private IRandomLineGenerator _randomLineGenerator;

        public StudentFileCreator(IWriter stream, IRandomLineGenerator randomLineGenerator)
        {
            _stream = stream;
            _randomLineGenerator = randomLineGenerator;
        }

        public List<string> GetSurnames()
        {
            return [
                "Сычев",
                "Пригожин",
                "Прокофьев",
                "Арутюнов",
                "Бартенев"
            ];
        }

        public List<string> GetNames()
        {
            return [
                "Сергей",
                "Иван",
                "Леонид",
                "Георгий",
                "Вячеслав"
            ];
        }

        public void Create(int fileLength)
        {
            for (int i = 0; i < fileLength; ++i)
            {
                _stream.WriteLine(_randomLineGenerator.GetRandomLine());
            }
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
