namespace StatisticsAPI.Models;

partial class FileMetadata
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public required string StoragePath { get; set; }
    public int Size { get; set; }
    public DateTime CreatedAt { get; set; }

}
