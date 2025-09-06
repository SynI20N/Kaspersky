using TrackSense.API.AlertService.Models;
using TrackSense.API.MetricGateway.Models;

namespace TrackSense.API.AlertService.Services;
public class MetricAnalyzerService : IMetricAnalyzerService
{
    private readonly ILogger<MetricAnalyzerService> _logger;
    private readonly IRulesRepository _rulesRepository;
    private readonly IEventsRepository _eventsRepository;
    private readonly IConfiguration _configuration;
    private readonly Dictionary<long, TimedMetricEvent> _trackedMetrics;
    public MetricAnalyzerService(
        ILogger<MetricAnalyzerService> logger,
        IRulesRepository rulesRepository,
        IEventsRepository eventsRepository,
        IConfiguration configuration,
        Dictionary<long, TimedMetricEvent> metricEventDictionary)
    {
        _logger = logger;
        _rulesRepository = rulesRepository;
        _eventsRepository = eventsRepository;
        _configuration = configuration;
        _trackedMetrics = metricEventDictionary;
    }

    public async Task MetricAnalyzeAsync(RawMetric metric)
    {
        List<AlertRule> rules = await _rulesRepository.GetAlertRulesByImeiAsync(metric.Imei);

        if (rules == null)
        {
            return;
        }

        foreach (var r in rules)
        {
            object obj = metric.GetType().GetProperty(r.ValueName).GetValue(metric, null);
            bool converted = float.TryParse(obj.ToString(), out float value);

            if (!converted)
            {
                continue;
            }

            foreach (var entry in _trackedMetrics)
            {
                _logger.LogInformation($"[{entry.Key}]={entry.Value}");
            }
            bool cmp = OperatorMaps.FuncMappings[r.Operator](value, r.CriticalValue);

            try
            {
                KeyValuePair<long, TimedMetricEvent> record = _trackedMetrics.First(
                    me => me.Key == metric.Imei && me.Value.Event.IMEI == metric.Imei);
                if (cmp)
                {
                    await RaiseEventStatus(metric, r, record);
                }
                else
                {
                    await LowerEventStatus(metric, r, record);
                }
            }
            catch (InvalidOperationException)
            {
                if (cmp)
                {
                    await BeginEventTracking(metric, r);
                }
                _logger.LogInformation($"Beginning to track metric {metric}");
            }
        }
    }

    private async Task BeginEventTracking(RawMetric metric, AlertRule r)
    {
        AlertEvent e;
        if (await _eventsRepository.EventExistsAsync(r.ID, metric.Imei))
        {
            await _eventsRepository.ChangeStatusByRuleAndImeiAsync(r.ID, metric.Imei, AlertEventStatus.Unknown);

            e = await _eventsRepository.GetByRuleAndImeiAsync(r.ID, metric.Imei);
        }
        else
        {
            e = new AlertEvent
            {
                GPS = metric.GPS,
                Status = AlertEventStatus.Unknown,
                AlertRuleId = r.ID,
                IMEI = metric.Imei
            };
            await _eventsRepository.InsertEventAsync(e);
        }
        _trackedMetrics[metric.Imei] = new TimedMetricEvent(metric, e, metric.DateTime);
    }

    private async Task RaiseEventStatus(RawMetric metric, AlertRule r, KeyValuePair<long, TimedMetricEvent> record)
    {
        int interval = int.Parse(_configuration["EventRaiseInterval"]);
        AlertEventStatus status = await _eventsRepository.GetStatusByRuleAndImeiAsync(r.ID, metric.Imei);
        if ((DateTime.Now - record.Value.Time).Seconds > interval && status == AlertEventStatus.Unknown)
        {
            await _eventsRepository.ChangeStatusByRuleAndImeiAsync(r.ID, metric.Imei, AlertEventStatus.Pending);
            TimedMetricEvent metricEvent = _trackedMetrics[metric.Imei];
            metricEvent.Time = metric.DateTime;
            _trackedMetrics[metric.Imei] = metricEvent;
        }
    }

    private async Task LowerEventStatus(RawMetric metric, AlertRule r, KeyValuePair<long, TimedMetricEvent> record)
    {
        int interval = int.Parse(_configuration["EventLowerInterval"]);
        AlertEventStatus status = await _eventsRepository.GetStatusByRuleAndImeiAsync(r.ID, metric.Imei);
        if ((DateTime.Now - record.Value.Time).Seconds > interval && status == AlertEventStatus.Active)
        {
            await _eventsRepository.ChangeStatusByRuleAndImeiAsync(r.ID, metric.Imei, AlertEventStatus.Deactivate);
            _trackedMetrics.Remove(metric.Imei);
        }
    }
}