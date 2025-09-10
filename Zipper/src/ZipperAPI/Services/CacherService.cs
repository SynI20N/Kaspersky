
using ZipperAPI.Models;

namespace ZipperAPI.Services;

public class CacherService : ICacher
{
    private readonly Dictionary<string, ProcessInfo> _infos = new();
    public void Cache(string files, ProcessInfo info)
    {
        if(_infos.ContainsKey(files))
        {
            return;
        }
        else
        {
            _infos.Add(files, info);
        }
    }

    public bool TryGetCached(string files, out ProcessInfo? info)
    {
        if(!_infos.ContainsKey(files))
        {
            info = null;
            return false;
        }
        info = _infos[files];
        return true;
    }
}
