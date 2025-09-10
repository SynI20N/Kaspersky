using System.Data.Common;

namespace StatisticsAPI.Services;

public interface IDbCsvStreamer
{
    public Task DownloadCsvFormat(DbDataReader reader, StreamWriter writer, CancellationToken token);
}
