using CliFx.Attributes;
using CliFx.Infrastructure;

namespace ZipperCLI.Exe.Commands;

[Command("status", Description = "Check status of archive process")]
public class StatusCommand : BaseZipperCommand
{
    public StatusCommand(Uri baseAddress) : base(baseAddress)
    {
    }

    [CommandParameter(0, Description = "Process Id", IsRequired = true)]
    public int ProcessId { get; set; }

    public override async ValueTask ExecuteAsync(IConsole console)
    {
        using var client = new HttpClient { BaseAddress = new Uri("http://localhost:8080/") };
        var status = await client.GetStringAsync($"zipper/archive/status/{ProcessId}");
        await console.Output.WriteLineAsync(status);
    }
}
