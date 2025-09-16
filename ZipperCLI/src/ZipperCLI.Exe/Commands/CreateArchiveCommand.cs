using System.Net.Http.Json;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace ZipperCLI.Exe.Commands;

[Command("create-archive", Description = "Create archive from given files")]
public class CreateArchiveCommand : BaseZipperCommand
{
    private readonly string _archiveEndpoint;

    public CreateArchiveCommand(Uri baseAddress, string version, IConfiguration config) 
        : base(baseAddress, version)
    {
        var endpoints = config.GetSection("Endpoints");
        _archiveEndpoint = endpoints["Create-Archive"];
    }

    [CommandParameter(0, Description = "Files to archive", IsRequired = true)]
    public string[] Files { get; set; } = Array.Empty<string>();

    public override async ValueTask ExecuteAsync(IConsole console)
    {
        var response = await _httpClient.PostAsJsonAsync(_version + _archiveEndpoint, Files);
        var content = await response.Content.ReadAsStringAsync();
        await console.Output.WriteLineAsync(content);
    }
}
