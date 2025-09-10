namespace StatisticsAPI.Services;

public interface IFilePersistence
{
    public Task<string> UploadAsync(IFormFile file, CancellationToken token = default);
    public Task DownloadAsync(string fileId, Stream to, CancellationToken token = default);
    public Task ValidateAsync(Stream fileStream, CancellationToken token = default);
}
