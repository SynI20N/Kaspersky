using TrackSense.API.MetricGateway.Models;

namespace TrackSense.API.MetricGateway.Services;

public interface IKafkaSender
{
    Task ProduceAsync(RawMetric metric);
}