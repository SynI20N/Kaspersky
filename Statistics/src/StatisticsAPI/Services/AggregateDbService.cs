using System.Data.Common;
using StatisticsDatabase.Repositories;

namespace StatisticsAPI.Services
{
    public class AggregateDbService : IAggregator, IDbCsvStreamer
    {
        private readonly ILogger<AggregateDbService> _logger;
        private readonly IAggregateRepository _agg_stats;

        public AggregateDbService(
            ILogger<AggregateDbService> logger,
            IAggregateRepository agg_stats)
        {
            _logger = logger;
            _agg_stats = agg_stats;
        }

        public async Task AggregateByProductAndSeverity(Stream to, CancellationToken token)
        {
            await using var reader = await _agg_stats.AggregateTop10ErrorCodesByProductAndSeverity();
            await using var writer = new StreamWriter(to);
            await DownloadCsvFormat(reader, writer, token);
        }

        public async Task AggregateByProductAndVersionAsync(Stream to, CancellationToken token)
        {
            await using var reader = await _agg_stats.AggregateByProductAndVersion();
            await using var writer = new StreamWriter(to);
            await DownloadCsvFormat(reader, writer, token);
        }

        public async Task AggregateBySeverityAsync(Stream to, CancellationToken token)
        {
            await using var reader = await _agg_stats.AggregateBySeverityAndSort();
            await using var writer = new StreamWriter(to);
            await DownloadCsvFormat(reader, writer, token);
        }

        public async Task AggregateCustom(Stream to, CancellationToken token)
        {
            await using var reader = await _agg_stats.AggregateByHourProductVersionMaxErrorCode();
            await using var writer = new StreamWriter(to);
            await DownloadCsvFormat(reader, writer, token);
        }

        public async Task DownloadCsvFormat(DbDataReader from, StreamWriter to, CancellationToken token)
        {
            for (int i = 0; i < from.FieldCount; i++)
            {
                if (token.IsCancellationRequested)
                {
                    throw new Exception("uploading cancelled");
                }
                if (i > 0) await to.WriteAsync(",");
                await to.WriteAsync(from.GetName(i));
            }
            await to.WriteLineAsync();

            while (await from.ReadAsync())
            {
                for (int i = 0; i < from.FieldCount; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        throw new Exception("uploading cancelled");
                    }
                    if (i > 0) await to.WriteAsync(",");
                    var val = from.IsDBNull(i) ? "" : from.GetValue(i).ToString();
                    await to.WriteAsync($"{val}");
                }
                await to.WriteLineAsync();
            }
        }
    }
}
