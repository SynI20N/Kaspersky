using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using ZipperAPI.Controllers;
using ZipperAPI.Services;

namespace ZipperAPI.Tests;

public class InfoControllerTestSetup
{
    public InfoControllerTestSetup()
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
                           ?? throw new InvalidOperationException("Test project folder not configured.");
        var mockEnvironment = new Mock<IWebHostEnvironment>();
        mockEnvironment
            .Setup(m => m.ContentRootPath)
            .Returns(contentRoot);

        serviceCollection.AddSingleton<IConfiguration>(configuration);
        serviceCollection.AddSingleton(mockEnvironment.Object);
        serviceCollection.AddTransient<IFolderService, FolderService>();
        serviceCollection.AddTransient<InfoController, InfoController>();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public ServiceProvider ServiceProvider { get; private set; }
}
