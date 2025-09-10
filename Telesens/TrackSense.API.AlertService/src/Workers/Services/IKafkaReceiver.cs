using TrackSense.API.MetricGateway.Models;

namespace TrackSense.API.AlertService.Workers.Services;

public interface IKafkaReceiver
{
    public RawMetric Receive(CancellationToken ct);
}