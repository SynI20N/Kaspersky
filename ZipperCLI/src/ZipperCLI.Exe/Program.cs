using CliFx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZipperCLI.Exe.Commands;

namespace ZipperCLI;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        CliApplication app = BuildApplication();

        if (args.Length > 0)
        {
            return await app.RunAsync(args);
        }

        Console.WriteLine("ZipperCLI interactive mode. Type 'help' for commands, 'exit' to quit.");

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            var splitArgs = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            try
            {
                await app.RunAsync(splitArgs);
            }
            catch
            {

            }
        }

        return 0;
    }

    private static CliApplication BuildApplication()
    {
        // Setup DI container
        var services = new ServiceCollection();

        // Получаем значение из переменной окружения
        string basePathFromEnv = Environment.GetEnvironmentVariable("CONFIG_BASE_PATH")
            ?? throw new Exception("Переменная окружения CONFIG_BASE_PATH не установлена");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePathFromEnv)
            .AddJsonFile(
                 path: "appsettings.json",
                 optional: false,
                 reloadOnChange: true)
           .Build();

        // Получаем настройки сервера из конфигурации
        var server = configuration["Server"];
        var port = configuration["Port"];
        var apiVersion = configuration["ApiVersion"]
            ?? "api.zipper/v1";

        // Формируем полный URI
        var baseUri = $"{server}:{port}";
        var uri = new Uri(baseUri);

        // Регистрируем URI в DI контейнере
        services.AddSingleton(uri);
        services.AddSingleton(apiVersion);
        services.AddSingleton<IConfiguration>(configuration);

        // Register commands
        services.AddTransient<CreateArchiveCommand>();
        services.AddTransient<ListFilesCommand>();
        services.AddTransient<DownloadCommand>();
        services.AddTransient<StatusCommand>();
        services.AddTransient<RunCommand>();

        var serviceProvider = services.BuildServiceProvider();

        // Build CLI application with DI
        var app = new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .UseTypeActivator(serviceProvider.GetRequiredService)
            .Build();

        return app;
    }
}