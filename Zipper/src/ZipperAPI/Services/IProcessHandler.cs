using ZipperAPI.Models;

namespace ZipperAPI.Services;

public interface IProcessHandler
{
    public StatusType ProcessStatus(int processId);
    public int StartArchiveProcess(string[] files);
    public FileStream ProcessStream(int processId);
}
