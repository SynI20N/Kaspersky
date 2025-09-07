using StatisticsAPI.Dto;
using StatisticsAPI.Models;

namespace StatisticsAPI.Services;

public interface IFilePersistence
{
    public Task<string> UploadAsync(Stream fileStream, CancellationToken token = default);
    public Task Download(string fileId, Stream to, CancellationToken token = default);
    public Task Validate(Stream fileStream, CancellationToken token = default);
}
