using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace ZipperCLI.Exe.Commands;

[Command("status", Description = "Check status of archive process")]
public class StatusCommand : BaseZipperCommand
{
    private readonly string _statusEndpoint;
    public StatusCommand(Uri baseAddress, string version, IConfiguration config) 
        : base(baseAddress, version)
    {
        var endpoints = config.GetSection("Endpoints");
        _statusEndpoint = endpoints["Status-Archive"];
    }

    [CommandParameter(0, Description = "Process Id", IsRequired = true)]
    public int ProcessId { get; set; }

    public override async ValueTask ExecuteAsync(IConsole console)
    {
        var status = await _httpClient.GetStringAsync(_version + $"{_statusEndpoint}/{ProcessId}");
        await console.Output.WriteLineAsync(status);
    }
}
