using System.Net.Http.Json;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace ZipperCLI.Exe.Commands;

[Command("list", Description = "List available files on server")]
public class ListFilesCommand : BaseZipperCommand
{
    public ListFilesCommand(Uri baseAddress) : base(baseAddress)
    {
    }

    public override async ValueTask ExecuteAsync(IConsole console)
    {
        var files = await _httpClient.GetFromJsonAsync<string[]>("zipper/files");
        if (files != null && files.Length > 0)
            await console.Output.WriteLineAsync(string.Join(' ', files));
    }
}
