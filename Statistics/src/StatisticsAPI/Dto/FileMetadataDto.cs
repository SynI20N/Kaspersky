namespace StatisticsAPI.Dto;

public partial class FileMetadataDto
{
    public string Id { get; set; }
    public string? OriginalFileName { get; set; }
    public long Size { get; set; }
    public string? DownloadUrl { get; set; }

    public FileMetadataDto(string id, string? originalFileName, long size, string? downloadUrl)
    {
        Id = id;
        OriginalFileName = originalFileName;
        Size = size;
        DownloadUrl = downloadUrl;
    }
}
