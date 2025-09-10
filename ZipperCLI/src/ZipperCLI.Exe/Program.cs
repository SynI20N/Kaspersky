using CliFx;
using Microsoft.Extensions.DependencyInjection;
using ZipperCLI.Exe.Commands;

namespace ZipperCLI;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        CliApplication app = StartApplication();

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

    private static CliApplication StartApplication()
    {
        // Setup DI container
        var services = new ServiceCollection();

        // Register Uri and HttpClient
        services.AddSingleton(new Uri("http://localhost:8080"));

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