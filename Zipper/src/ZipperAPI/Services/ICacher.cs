using ZipperAPI.Models;

namespace ZipperAPI.Services;

public interface ICacher
{
    public bool TryGetCached(string files, out ProcessInfo? info);
    public void Cache(string files, ProcessInfo info);
}
