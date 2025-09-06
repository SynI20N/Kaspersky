using Microsoft.EntityFrameworkCore;
using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService.Services;
public class NotificationService : INotificationService
{
    private readonly IEnumerable<ISender> _senders;
    private readonly ILogger<NotificationService> _logger;
    private readonly AlertServiceContext _context;
    private readonly Dictionary<int, Retry> _trialsDict;
    private readonly IEventsRepository _eventsRepository;

    public NotificationService(
        IEnumerable<ISender> senders,
        ILogger<NotificationService> logger,
        AlertServiceContext context,
        Dictionary<int, Retry> trialsDict,
        IEventsRepository eventsRepository)
    {
        _senders = senders;
        _logger = logger;
        _context = context;
        _trialsDict = trialsDict;
        _eventsRepository = eventsRepository;
    }

    public async Task NotifyAsync()
    {
        List<AlertEvent> retryEvents = await _context.Events
            .Where(e => e.Status == AlertEventStatus.NotNotified)
            .ToListAsync();
        foreach (var e in retryEvents)
        {
            try
            {
                KeyValuePair<int, Retry> retry = _trialsDict.First(t => t.Key == e.ID);
                if(retry.Value.Count <= 0)
                {
                    _logger.LogError($"Event {e.ID} could not be notified in desired amount of retries");
                    await _eventsRepository.ChangeStatusByRuleAndImeiAsync(e.AlertRuleId, e.IMEI, AlertEventStatus.Unknown);
                    continue;
                }
                _logger.LogInformation($"{retry.Value.Count} retries left");
                bool allNotified = true;
                foreach (var sendFunction in retry.Value.Funcs)
                {
                    bool notified = await sendFunction();
                    if(!notified)
                    {
                        allNotified = false;
                    }
                }
                _trialsDict[e.ID] = new Retry(retry.Value.Funcs, retry.Value.Count - 1);
                if(allNotified)
                {
                    _trialsDict.Remove(e.ID);
                    await _eventsRepository.ChangeStatusByRuleAndImeiAsync(e.AlertRuleId, e.IMEI, AlertEventStatus.Active);
                }
            }
            catch (InvalidOperationException)
            {
                _logger.LogError($"Event {e.ID} was not notified but doesn't exist in the dictionary");
                await _eventsRepository.ChangeStatusByRuleAndImeiAsync(e.AlertRuleId, e.IMEI, AlertEventStatus.Unknown);
            }
        }
        foreach(var s in _senders)
        {
            await s.SendAllAsync();
        }
    }
}