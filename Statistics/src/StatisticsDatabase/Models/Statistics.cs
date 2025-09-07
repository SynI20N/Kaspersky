namespace StatisticsDatabase.Models;

public class Statistics
{
    public DateTime TimeSpamp { get; set; }
    public SeverityType Severity { get; set; }
    public string? Product { get; set; }
    public string? Version { get; set; }
    public byte ErrorCode { get; set; }
}
