using TrackSense.API.AlertService.Models;
using TrackSense.API.AlertService.Services.CRUD;

namespace TrackSense.API.AlertService.Repositories;

public static class ServiceCollectionExtensions
{
    public static void AddCrudServices(this IServiceCollection services)
    {
        services.AddScoped<ICrudService<NotificationGroup>, NotificationGroupService>();
        services.AddScoped<ICrudService<TelegramChat>, TelegramChatService>();
        services.AddScoped<ICrudService<Email>, EmailService>();
        services.AddScoped<ICrudService<AlertRule>, AlertRuleService>();
        services.AddScoped<ICrudService<AlertEvent>, AlertEventService>();
        services.AddScoped<ICrudService<AlertRuleNotificationGroup>, AlertRuleNotificationGroupService>();
    }
}