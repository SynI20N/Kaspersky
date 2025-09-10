using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatisticsDatabase.Models;

public class Statistic
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public DateTime TimeStamp { get; set; }
    [Required]
    public SeverityType Severity { get; set; }
    [Required]
    public string? Product { get; set; }
    [Required]
    public string? Version { get; set; }
    [Required]
    public byte ErrorCode { get; set; }

    public Statistic()
    {

    }

    public Statistic(StatisticFile file)
    {
        TimeStamp = file.TimeStamp;
        Severity = file.Severity;
        Product = file.Product;
        Version = file.Version;
        ErrorCode = file.ErrorCode;
    }
}
