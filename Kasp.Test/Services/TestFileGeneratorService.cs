using Kasp.Test.Interfaces;

namespace Kasp.Test.Classes
{
    public class TestFileGeneratorService : ITestFileGeneratorService
    {
        public void GenerateTestFiles(string baseDir, int subDirsCount, int filesPerSubDir)
        {
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            Random random = new Random();

            for (int i = 0; i < subDirsCount; i++)
            {
                string subDirPath = Path.Combine(baseDir, $"subdir_{i + 1}");
                Directory.CreateDirectory(subDirPath);
                Console.WriteLine($"Создан подкаталог: {subDirPath}");

                for (int j = 0; j < filesPerSubDir; j++)
                {
                    string fileName = Path.Combine(subDirPath, $"testFile{j + 1}.log");
                    string fileContent = GenerateRandomFileContent(random);

                    File.WriteAllText(fileName, fileContent);
                    Console.WriteLine($"Создан файл: {fileName}");
                }
            }

            Console.WriteLine("Все тестовые файлы и подкаталоги были успешно созданы.");
        }

        private string GenerateRandomFileContent(Random random)
        {
            string[] testPatterns = new string[]
            {
            "password=%TmySecret123c%\nThis is a log entry.\nThis contains the word master and slave.\n",
            "Here is a license key: ABCDE-12345-FGH67-JK89F\npassword:%anotherPassword123%\n",
            "*****-*****-*****-*****\nThe whitelist should be updated.\n",
            "masterpassword=%mySecret123%\nSanitycheck was performed.\nmaster = %secretpassword%\n",
            "manhours=1000\nThe master is in charge.\nGrandfathered system running.\n"
            };

            return testPatterns[random.Next(testPatterns.Length)];
        }
    }

}
