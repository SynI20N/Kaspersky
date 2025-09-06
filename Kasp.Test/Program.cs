using Microsoft.Extensions.DependencyInjection;
using Kasp.Test.Interfaces;
using Kasp.Test.Classes;

//class Program
//{
//    //static void Main(string[] args)
//    //{
//    //    // Create a ServiceCollection and add services
//    //    var serviceProvider = new ServiceCollection()
//    //        .AddSingleton<IContentReplacementService, ContentReplacementService>()
//    //        .AddSingleton<IFileProcessingService, FileProcessingService>()
//    //        .AddSingleton<ITestFileGeneratorService, TestFileGeneratorService>()
//    //        .BuildServiceProvider();

//    //    // Get the service instances
//    //    var fileProcessingService = serviceProvider.GetService<IFileProcessingService>();
//    //    var testFileGeneratorService = serviceProvider.GetService<ITestFileGeneratorService>();

//    //    // Test Mode check
//    //    string? testMode = Environment.GetEnvironmentVariable("GEN_MODE");

//    //    if (!string.IsNullOrEmpty(testMode) && testMode.Equals("1", StringComparison.OrdinalIgnoreCase))
//    //    {
//    //        Console.WriteLine("Запуск генератора тестовых файлов...");
//    //        testFileGeneratorService.GenerateTestFiles("test_files", 5, 10); // Генерация файлов
//    //        return;
//    //    }

//    //    if (args.Length < 1)
//    //    {
//    //        Console.WriteLine("Ошибка: не указан путь к директории.");
//    //        return;
//    //    }

//    //    string directoryPath = args[0];

//    //    if (!Directory.Exists(directoryPath))
//    //    {
//    //        Console.WriteLine("Указанный путь не существует.");
//    //        return;
//    //    }

//    //    fileProcessingService.ProcessDirectory(directoryPath);
//    //}
//}
public class HelloWorld
{
    public static void Main(string[] args)
    {
        DataProcessor<string> process = new DataProcessor<string>();
        process.Process(123);
    }
}

public class Processor<T>
{
    public void Process(T value)
    {
        Console.Write(value.GetType().Name);
    }
}

public class DataProcessor<U> : Processor<int>
{

}

