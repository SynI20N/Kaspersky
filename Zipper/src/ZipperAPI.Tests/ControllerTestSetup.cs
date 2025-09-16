using Moq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZipperAPI.Controllers;
using ZipperAPI.Services;

namespace ZipperAPI.Tests;

public class ControllerTestSetup
{
    public ControllerTestSetup()
    {
        var serviceCollection = new ServiceCollection().AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                 path: "appsettings.json",
                 optional: false,
                 reloadOnChange: true)
           .Build();

        var contentRoot = Environment.GetEnvironmentVariable("TEST_PROJECT_FOLDER")
                           ?? throw new InvalidOperationException("Test project folder not configured");
        var mockEnvironment = new Mock<IWebHostEnvironment>();
        mockEnvironment
            .Setup(m => m.ContentRootPath)
            .Returns(contentRoot);

        serviceCollection.AddSingleton<IConfiguration>(configuration);
        serviceCollection.AddSingleton(mockEnvironment.Object);

        var factory = serviceProvider.GetService<ILoggerFactory>();
        var logger = factory.CreateLogger<ProcessController>();
        var logger2 = factory.CreateLogger<ProcessService>();
        var logger3 = factory.CreateLogger<DownloadController>();
        var logger4 = factory.CreateLogger<InfoController>();

        serviceCollection.AddSingleton<IProcessHandler, ProcessService>();
        serviceCollection.AddSingleton<IFolderService, FolderService>();
        serviceCollection.AddSingleton<ICacher, CacherService>();

        serviceCollection.AddTransient<ProcessController, ProcessController>();
        serviceCollection.AddTransient<DownloadController, DownloadController>();
        serviceCollection.AddTransient<InfoController, InfoController>();


        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public ServiceProvider ServiceProvider { get; private set; }
}
