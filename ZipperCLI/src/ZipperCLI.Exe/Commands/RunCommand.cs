using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace ZipperCLI.Exe.Commands;

[Command("run", Description = "Create archive, wait for completion, then download")]
public class RunCommand : BaseZipperCommand
{
    private readonly string _statusEndpoint;
    private readonly string _startEndpoint;
    private readonly string _downloadEndpoint;
    public RunCommand(Uri baseAddress, string version, IConfiguration config) 
        : base(baseAddress, version)
    {
        var endpoints = config.GetSection("Endpoints");
        _statusEndpoint = endpoints["Status-Archive"];
        _startEndpoint = endpoints["Create-Archive"];
        _downloadEndpoint = endpoints["Download-Archive"];
    }

    [CommandParameter(0, Description = "Destination folder", IsRequired = true)]
    public string Destination { get; set; } = string.Empty;

    [CommandParameter(1, Description = "Files to archive", IsRequired = true)]
    public string[] Files { get; set; } = Array.Empty<string>();


    public override async ValueTask ExecuteAsync(IConsole console)
    {
        // create
        var response = await _httpClient.PostAsJsonAsync(_version + _startEndpoint, Files);
        if (!response.IsSuccessStatusCode) return;

        var id = await response.Content.ReadFromJsonAsync<int>();
        await console.Output.WriteLineAsync(id.ToString());

        // poll status
        string status;
        do
        {
            await Task.Delay(500);
            status = await _httpClient.GetStringAsync(_version + $"{_statusEndpoint}/{id}");
            await console.Output.WriteLineAsync(status);
        } while (status.Equals("InProgress", StringComparison.OrdinalIgnoreCase));

        if (!status.Equals("Completed", StringComparison.OrdinalIgnoreCase)) return;

        // download
        var download = await _httpClient.GetAsync(_version + $"{_downloadEndpoint}/{id}");
        if (!download.IsSuccessStatusCode) return;

        var filePath = Path.Combine(Destination, $"archive_{id}.zip");
        await using var fs = File.Create(filePath);
        await download.Content.CopyToAsync(fs);
        await console.Output.WriteLineAsync(filePath);
    }
}

