using CliFx.Infrastructure;
using ZipperCLI.Exe.Commands;

public class CommandTests
{
    [Fact]
    public async Task ListFilesCommand_Should_Print_NoFiles_When_Empty()
    {
        // Arrange
        var console = new FakeInMemoryConsole();
        var addr = new Uri("http://localhost:8080/");
        var cmd = new ListFilesCommand(addr);

        // Act
        await cmd.ExecuteAsync(console);

        // Assert
        var output = console.ReadOutputString();
        Assert.Contains("big1.dll big2.dll empty1.txt empty2.txt long.txt\r\n", output);
    }

    [Fact]
    public async Task CreateArchiveCommand_Should_Print_Do_Not_Exist()
    {
        // Arrange
        var console = new FakeInMemoryConsole();
        var addr = new Uri("http://localhost:8080/");

        var cmd = new CreateArchiveCommand(addr)
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
        var addr = new Uri("http://localhost:8080/");
        var dest = Path.GetTempPath();

        var cmd = new RunCommand(addr)
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
