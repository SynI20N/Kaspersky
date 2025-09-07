using System.Formats.Asn1;
using System.Globalization;
using System.Reflection.PortableExecutable;
using Azure;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using StatisticsAPI.Dto;
using StatisticsAPI.Models;
using StatisticsDatabase.Context;
using StatisticsDatabase.Models;
using StatisticsDatabase.Repositories;

namespace StatisticsAPI.Services;

class FilePersistenceDbService : IFilePersistence
{
    private readonly string _storagePath;
    private readonly long _maxFileSize;
    private readonly ILogger<FilePersistenceDbService> _logger;
    private readonly CrudRepository<Statistics> _statistics;
    private readonly StatisticsDatabaseContext _context;

    public FilePersistenceDbService(
        IConfiguration configuration,
        ILogger<FilePersistenceDbService> logger,
        CrudRepository<Statistics> statistics,
        StatisticsDatabaseContext context)
    {
        var section = configuration.GetSection("Storage");

        _storagePath = section.GetValue<string>("Path") ?? "Data/uploads";
        _maxFileSize = section.GetValue<long>("MaxAllowedSize");
        _logger = logger;
        _statistics = statistics;
        _context = context;
    }

    public async Task Download(string id, Stream to, CancellationToken token)
    {
        using var conn = _context.Database.GetDbConnection();
        conn.Open();

        var sql = _context.Statistics
            .Select(s => new { s.TimeSpamp, s.Severity, s.Product, s.Version, s.ErrorCode })
            .ToQueryString();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        using var reader = await cmd.ExecuteReaderAsync();
        await using var writer = new StreamWriter(to);

        for (int i = 0; i < reader.FieldCount; i++)
        {
            if (i > 0) await writer.WriteAsync(",");
            await writer.WriteAsync(reader.GetName(i));
        }
        await writer.WriteLineAsync();

        while (await reader.ReadAsync())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (i > 0) await writer.WriteAsync(",");
                var val = reader.IsDBNull(i) ? "" : reader.GetValue(i).ToString()?.Replace("\"", "\"\"");
                await writer.WriteAsync($"\"{val}\"");
            }
            await writer.WriteLineAsync();
        }
    }

    public async Task<string> UploadAsync(Stream fileStream, CancellationToken token)
    {
        var s = new StreamReader(fileStream);
        var csv = new CsvReader(s, CultureInfo.InvariantCulture);
        IEnumerable<Statistics> stats = csv.GetRecords<Statistics>();
        foreach (var record in stats)
        {
            if(token.IsCancellationRequested)
            {
                throw new Exception("uploading cancelled");
            }
            await _statistics.AddAsync(record);
        }
        return "StatisticsTable";
    }

    public async Task Validate(Stream fileStream, CancellationToken token)
    {
        //implement later
    }
}
