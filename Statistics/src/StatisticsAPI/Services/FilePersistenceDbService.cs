using System.Globalization;
using CsvHelper;
using StatisticsDatabase.Models;
using StatisticsDatabase.Repositories;

namespace StatisticsAPI.Services;

class FilePersistenceDbService : IFilePersistence
{
    private readonly ILogger<FilePersistenceDbService> _logger;
    private readonly ICrudRepository<Statistic> _statistics;
    private readonly long _maxFileSize;

    public FilePersistenceDbService(
        ILogger<FilePersistenceDbService> logger,
        ICrudRepository<Statistic> statistics,
        IConfiguration configuration)
    {
        _logger = logger;
        _statistics = statistics;
        _maxFileSize = configuration.GetSection("Storage").GetValue<long>("MaxAllowedSize");
    }

    public async Task DownloadAsync(string id, Stream to, CancellationToken token)
    {
        await using var writer = new StreamWriter(to);
        await writer.WriteLineAsync("TimeStamp,Severity,Product,Version,ErrorCode");

        await foreach (var s in _statistics.GetAllAsync())
        {
            if (token.IsCancellationRequested) throw new Exception("downloading cancelled");
            string line = string.Join(",",
                s.TimeStamp.ToString("O"),   // ISO 8601 for clarity
                s.Severity.ToString(),
                s.Product,
                s.Version,
                s.ErrorCode.ToString()
            );

            await writer.WriteLineAsync(line);
        }

        await writer.FlushAsync();
    }

    public async Task<string> UploadAsync(IFormFile file, CancellationToken token)
    {
        if (file == null) { throw new ArgumentException("file empty"); }
        if (file.Length == 0 || file.Length > _maxFileSize)
        {
            _logger.LogWarning("Uploaded incorrent file");

            throw new FormatException("invalid file size");
        }
        using (Stream stream = file.OpenReadStream())
        {
            await ValidateAsync(stream, token);
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);
            else
            {
                _logger.LogError("Could not seek a stream of input file");

                throw new Exception("seek error");
            }

            var s = new StreamReader(stream);
            var csv = new CsvReader(s, CultureInfo.InvariantCulture);
            IEnumerable<StatisticFile> stats = csv.GetRecords<StatisticFile>();
            foreach (var record in stats)
            {
                if (token.IsCancellationRequested)
                {
                    throw new Exception("uploading cancelled");
                }
                Statistic stat = new Statistic(record);
                await _statistics.AddAsync(stat);
            }
        }
        return "StatisticsTable";
    }

    public async Task ValidateAsync(Stream fileStream, CancellationToken token)
    {
        //implement later
    }
}
