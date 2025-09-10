using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleIOExe.Interfaces;

namespace SimpleIOExe.Services
{
    internal class RandomLineGenerator : IRandomLineGenerator
    {
        public static int MaxAge = 80;

        Random _rnd;

        public RandomLineGenerator(Random rnd)
        {
            _rnd = rnd;
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

        public int GetRandomAge()
        {
            return _rnd.Next(MaxAge);
        }

        public string GetRandomSurname()
        {
            List<string> surnames = GetSurnames();
            return surnames[_rnd.Next(surnames.Count)];
        }

        public string GetRandomName()
        {
            List<string> names = GetNames();
            return names[_rnd.Next(names.Count)];
        }

        public string GetRandomLine()
        {
            return $"{GetRandomSurname()} {GetRandomName()} {GetRandomAge()}";
        }
    }
}
