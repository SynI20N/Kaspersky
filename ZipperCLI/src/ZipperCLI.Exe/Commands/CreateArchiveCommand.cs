using System.Net.Http.Json;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace ZipperCLI.Exe.Commands;

[Command("create-archive", Description = "Create archive from given files")]
public class CreateArchiveCommand : BaseZipperCommand
{
    public CreateArchiveCommand(Uri baseAddress) : base(baseAddress)
    {
    }

    [CommandParameter(0, Description = "Files to archive", IsRequired = true)]
    public string[] Files { get; set; } = Array.Empty<string>();

    public override async ValueTask ExecuteAsync(IConsole console)
    {
        var response = await _httpClient.PostAsJsonAsync("zipper/archive/start", Files);
        var content = await response.Content.ReadAsStringAsync();
        await console.Output.WriteLineAsync(content);
    }
}