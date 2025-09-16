using CliFx.Infrastructure;
using Microsoft.Extensions.Configuration;
using ZipperCLI.Exe.Commands;

public class CommandTests
{
    private readonly IConfiguration _config;
    private readonly Uri _addr;
    private readonly string _apiVersion;

    public CommandTests()
    {
        string basePathFromEnv = Environment.GetEnvironmentVariable("CONFIG_BASE_PATH")
            ?? throw new Exception("Переменная окружения CONFIG_BASE_PATH не установлена");

        _config = new ConfigurationBuilder()
            .SetBasePath(basePathFromEnv)
            .AddJsonFile(
                 path: "appsettings.json",
                 optional: false,
                 reloadOnChange: true)
           .Build();

        var server = _config["Server"];
        var port = _config["Port"];
        _apiVersion = _config["ApiVersion"];
        var baseUri = $"{server}:{port}";
        _addr = new Uri(baseUri);
    }

    [Fact]
    public async Task ListFilesCommand_Should_Print_Files()
    {
        // Arrange
        var console = new FakeInMemoryConsole();
        var cmd = new ListFilesCommand(_addr, _apiVersion, _config);

        // Act
        await cmd.ExecuteAsync(console);

        // Assert
        var output = console.ReadOutputString();
        Assert.Contains("200mb.csv big1.dll big2.dll empty1.txt empty2.txt Game.unity\r\n", output);
    }

    [Fact]
    public async Task CreateArchiveCommand_Should_Print_Do_Not_Exist()
    {
        // Arrange
        var console = new FakeInMemoryConsole();

        var cmd = new CreateArchiveCommand(_addr, _apiVersion, _config)
        {
            Files = new[] { "file1.txt", "file2.txt" }
        };

        // Act
        await cmd.ExecuteAsync(console);

        // Assert
        string output = "";
        while(output == "")
        {
            output = console.ReadOutputString();
        }
        Assert.Contains("Some files do not exist: file1.txt, file2.txt\r\n", output);
    }

    [Fact]
    public async Task RunCommand_Should_Be_Ok()
    {
        // Arrange
        var console = new FakeInMemoryConsole();
        var dest = Path.GetTempPath();

        var cmd = new RunCommand(_addr, _apiVersion, _config)
        {
            Destination = dest,
            Files = new[] { "big1.dll", "big2.dll" }
        };

        // Act
        await cmd.ExecuteAsync(console);

        // Assert
        var output = console.ReadOutputString();
        Assert.DoesNotContain("Failed", output);
        Assert.Contains("Completed", output);
        Assert.Contains(dest, output);
    }
}
