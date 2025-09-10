namespace StatisticsDatabase.Models;

public class StatisticFile
{
    public DateTime TimeStamp { get; set; }
    public SeverityType Severity { get; set; }
    public string? Product { get; set; }
    public string? Version { get; set; }
    public byte ErrorCode { get; set; }
}
