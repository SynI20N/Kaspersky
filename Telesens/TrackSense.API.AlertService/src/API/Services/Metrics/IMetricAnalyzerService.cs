using TrackSense.API.MetricGateway.Models;

namespace TrackSense.API.AlertService.Services;

public interface IMetricAnalyzerService
{
    public Task MetricAnalyzeAsync(RawMetric metric);
} 