using CliFx.Attributes;
using CliFx.Infrastructure;

namespace ZipperCLI.Exe.Commands;

[Command("download", Description = "Download archive by process Id")]
public class DownloadCommand : BaseZipperCommand
{
    public DownloadCommand(Uri baseAddress) : base(baseAddress)
    {
    }

    [CommandParameter(0, Description = "Process Id", IsRequired = true)]
    public int ProcessId { get; set; }

    [CommandParameter(1, Description = "Destination path", IsRequired = true)]
    public string DestinationPath { get; set; } = string.Empty;

    public override async ValueTask ExecuteAsync(IConsole console)
    {
        var response = await _httpClient.GetAsync($"zipper/archive/download/{ProcessId}");
        if (response.IsSuccessStatusCode)
        {
            var filePath = Path.Combine(DestinationPath, $"archive_{ProcessId}.zip");
            await using var fs = File.Create(filePath);
            await response.Content.CopyToAsync(fs);
            await console.Output.WriteLineAsync(filePath);
        }
    }
}