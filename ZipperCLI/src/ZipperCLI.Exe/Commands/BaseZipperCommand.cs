using CliFx;
using CliFx.Infrastructure;

namespace ZipperCLI.Exe.Commands;

public abstract class BaseZipperCommand : ICommand
{
    protected Uri _baseAddress;
    protected HttpClient _httpClient;
    protected string _version;

    public BaseZipperCommand(Uri baseAddress, string apiVersion)
    {
        _baseAddress = baseAddress;
        _version = apiVersion;
        _httpClient = new HttpClient { BaseAddress = _baseAddress };
    }

    public abstract ValueTask ExecuteAsync(IConsole console);
}
