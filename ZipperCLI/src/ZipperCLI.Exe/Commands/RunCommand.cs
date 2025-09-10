using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Net.Http.Json;

namespace ZipperCLI.Exe.Commands;

[Command("run", Description = "Create archive, wait for completion, then download")]
public class RunCommand : BaseZipperCommand
{
    public RunCommand(Uri baseAddress) : base(baseAddress)
    {
    }

    [CommandParameter(0, Description = "Destination folder", IsRequired = true)]
    public string Destination { get; set; } = string.Empty;

    [CommandParameter(1, Description = "Files to archive", IsRequired = true)]
    public string[] Files { get; set; } = Array.Empty<string>();


    public override async ValueTask ExecuteAsync(IConsole console)
    {
        // create
        var response = await _httpClient.PostAsJsonAsync("zipper/archive/start", Files);
        if (!response.IsSuccessStatusCode) return;

        var id = await response.Content.ReadFromJsonAsync<int>();
        await console.Output.WriteLineAsync(id.ToString());

        // poll status
        string status;
        do
        {
            await Task.Delay(500);
            status = await _httpClient.GetStringAsync($"zipper/archive/status/{id}");
            await console.Output.WriteLineAsync(status);
        } while (status.Equals("InProgress", StringComparison.OrdinalIgnoreCase));

        if (!status.Equals("Completed", StringComparison.OrdinalIgnoreCase)) return;

        // download
        var download = await _httpClient.GetAsync($"zipper/archive/download/{id}");
        if (!download.IsSuccessStatusCode) return;

        var filePath = Path.Combine(Destination, $"archive_{id}.zip");
        await using var fs = File.Create(filePath);
        await download.Content.CopyToAsync(fs);
        await console.Output.WriteLineAsync(filePath);
    }
}

