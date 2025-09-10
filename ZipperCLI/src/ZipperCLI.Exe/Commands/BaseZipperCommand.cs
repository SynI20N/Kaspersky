using CliFx;
using CliFx.Infrastructure;

namespace ZipperCLI.Exe.Commands;

public abstract class BaseZipperCommand : ICommand
{
    protected Uri _baseAddress;
    protected HttpClient _httpClient;

    public BaseZipperCommand(Uri baseAddress)
    {
        _baseAddress = baseAddress;
        _httpClient = new HttpClient { BaseAddress = _baseAddress };
    }

    public abstract ValueTask ExecuteAsync(IConsole console);
}
