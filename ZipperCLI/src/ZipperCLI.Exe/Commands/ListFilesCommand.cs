using System.Net.Http.Json;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace ZipperCLI.Exe.Commands;

[Command("list", Description = "List available files on server")]
public class ListFilesCommand : BaseZipperCommand
{
    private readonly string _listFilesEndpoint;
    public ListFilesCommand(Uri baseAddress, string version, IConfiguration config) 
        : base(baseAddress, version)
    {
        var endpoints = config.GetSection("Endpoints");
        _listFilesEndpoint = endpoints["Get-Files"];
    }

    public override async ValueTask ExecuteAsync(IConsole console)
    {
        var files = await _httpClient.GetFromJsonAsync<string[]>(_version + _listFilesEndpoint);
        if (files != null && files.Length > 0)
            await console.Output.WriteLineAsync(string.Join(' ', files));
    }
}
