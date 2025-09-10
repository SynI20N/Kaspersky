namespace StatisticsAPI.Services;

public interface IAggregator
{
    public Task AggregateByProductAndSeverity(Stream to, CancellationToken token);
    public Task AggregateByProductAndVersionAsync(Stream to, CancellationToken token);
    public Task AggregateBySeverityAsync(Stream to, CancellationToken token);
    public Task AggregateCustom(Stream to, CancellationToken token);
}
