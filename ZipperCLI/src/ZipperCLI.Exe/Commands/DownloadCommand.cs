using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace ZipperCLI.Exe.Commands;

[Command("download", Description = "Download archive by process Id")]
public class DownloadCommand : BaseZipperCommand
{
    private readonly string _downloadArchiveEndpoint;
    public DownloadCommand(Uri baseAddress, string version, IConfiguration config) 
        : base(baseAddress, version)
    {
        var endpoints = config.GetSection("Endpoints");
        _downloadArchiveEndpoint = endpoints["Download-Archive"];
    }

    [CommandParameter(0, Description = "Process Id", IsRequired = true)]
    public int ProcessId { get; set; }

    [CommandParameter(1, Description = "Destination path", IsRequired = true)]
    public string DestinationPath { get; set; } = string.Empty;

    public override async ValueTask ExecuteAsync(IConsole console)
    {
        var response = await _httpClient.GetAsync(_version + $"{_downloadArchiveEndpoint}/{ProcessId}");
        if (response.IsSuccessStatusCode)
        {
            var filePath = Path.Combine(DestinationPath, $"archive_{ProcessId}.zip");
            await using var fs = File.Create(filePath);
            await response.Content.CopyToAsync(fs);
            await console.Output.WriteLineAsync(filePath);
        }
    }
}