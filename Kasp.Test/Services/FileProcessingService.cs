using Kasp.Test.Interfaces;

namespace Kasp.Test.Classes
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly IContentReplacementService _contentReplacementService;
        private readonly string _fileExtension = ".log";

        public FileProcessingService(IContentReplacementService contentReplacementService)
        {
            _contentReplacementService = contentReplacementService;
        }

        public void ProcessFile(string filePath, string extension)
        {
            if (Path.GetExtension(filePath).ToLower() != extension)
            {
                return;
            }

            Console.WriteLine($"Обработка файла: {filePath}");

            try
            {
                string content = File.ReadAllText(filePath);

                content = _contentReplacementService.ReplacePasswordPattern(content);
                content = _contentReplacementService.ReplaceLicenseKeyPattern(content);
                content = _contentReplacementService.ReplaceForbiddenWords(content);

                File.WriteAllText(filePath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке файла {filePath}: {ex.Message}");
            }
        }

        public void ProcessDirectory(string path)
        {
            try
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    ProcessFile(file, _fileExtension);
                }

                foreach (var subdir in Directory.GetDirectories(path))
                {
                    ProcessDirectory(subdir);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки директории {path}: {ex.Message}");
            }
        }
    }

}
