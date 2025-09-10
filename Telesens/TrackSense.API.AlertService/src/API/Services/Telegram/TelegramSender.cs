using Telegram.Bot;
using Microsoft.EntityFrameworkCore;
using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService.Services;

public class TelegramSender : ISender
{
    private readonly AlertServiceContext _context;
    private readonly ILogger<TelegramSender> _logger;
    private readonly IEventsRepository _eventsRepository;
    private readonly Dictionary<int, Retry> _trialsDict;
    private TelegramBotClient _bot;
    private int _retryCount;

    public TelegramSender(
        IConfiguration configuration,
        AlertServiceContext context,
        ILogger<TelegramSender> logger,
        IEventsRepository eventsRepository,
        Dictionary<int, Retry> trialsDict)
    {
        _context = context;
        _logger = logger;
        _eventsRepository = eventsRepository;
        _trialsDict = trialsDict;
        _bot = new TelegramBotClient(configuration["TelegramBot:Token"]);
        _retryCount = int.Parse(configuration["RetryCount"]);
    }

    public async Task<bool> SendAsync(string id, string message)
    {
        _logger.LogInformation($"Sending telegram message to {id}");
        try
        {
            await _bot.SendTextMessageAsync(id, message);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return false;
        }
    }

    public async Task SendAllAsync()
    {
        _logger.LogInformation("Sending messages to all telegram chats");
        List<AlertEvent> pendingEvents = await _context.Events
            .Where(e => e.Status == AlertEventStatus.Pending)
            .ToListAsync();
        AlertEventStatus resultStatus = AlertEventStatus.Active;
        foreach (var e in pendingEvents)
        {
            var alertGroups = await _context.GroupAlerts
                .Where(ag => ag.AlertRuleId == e.AlertRuleId)
                .ToListAsync();
            foreach (var ag in alertGroups)
            {
                var telegrams = await _context.Telegrams
                    .Where(t => t.NotificationGroupId == ag.NotificationGroup.ID)
                    .Select(t => t.TelegramChatId)
                    .ToListAsync();
                AlertRule rule = ag.AlertRule;
                var operString = OperatorMaps.StringMappings[rule.Operator];
                var typeString = AlertType.GetName(typeof(AlertType), rule.Type);
                var sendString = $"{typeString}: {rule.ValueName} value {operString} {rule.CriticalValue}";
                foreach (var tele in telegrams)
                {
                    bool sent = await SendAsync(tele, sendString);
                    if (!sent)
                    {
                        resultStatus = AlertEventStatus.NotNotified;
                        if (_trialsDict.ContainsKey(e.ID))
                        {
                            _trialsDict[e.ID].AddRetryFunction(async () => await SendAsync(tele, sendString));
                        }
                        else
                        {
                            _trialsDict[e.ID] = new Retry(async () => await SendAsync(tele, sendString), _retryCount);
                        }
                    }
                }
            }
            await _eventsRepository.ChangeStatusByRuleAndImeiAsync(e.AlertRuleId, e.IMEI, resultStatus);
        }
    }
}