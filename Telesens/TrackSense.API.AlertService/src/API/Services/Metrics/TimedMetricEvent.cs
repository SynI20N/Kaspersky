using TrackSense.API.AlertService.Models;
using TrackSense.API.MetricGateway.Models;

namespace TrackSense.API.AlertService.Services;

public struct TimedMetricEvent
{
    public RawMetric Metric { get; }
    public AlertEvent Event { get; }

    //only time can be modified 
    public DateTime Time { get; set; }

    public TimedMetricEvent(RawMetric metric, AlertEvent someEvent, DateTime time)
    {
        Metric = metric;
        Event = someEvent;
        Time = time;
    }
}