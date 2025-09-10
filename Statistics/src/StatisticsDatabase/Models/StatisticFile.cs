
namespace StatisticsDatabase.Models;

public class StatisticFile
{
    public DateTime TimeStamp { get; internal set; }
    public SeverityType Severity { get; internal set; }
    public string? Product { get; internal set; }
    public string? Version { get; internal set; }
    public byte ErrorCode { get; internal set; }
}
