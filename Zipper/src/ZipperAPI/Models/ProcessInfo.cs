using System.Diagnostics;

namespace ZipperAPI.Models;

public class ProcessInfo
{
    public int Id { get; set; }
    public StatusType Status { get; set; }
    public string DownloadPath { get; set; }
    public List<string>? Files { get; set; }
    public Process? Process { get; set; }
}
