    using Microsoft.Extensions.DependencyInjection;
using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService.Repositories;

public static class ServiceCollectionExtensions
{
    public static void AddCrudRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICrudRepository<NotificationGroup>, CrudRepository<NotificationGroup>>();
        services.AddTransient<ICrudRepository<TelegramChat>, CrudRepository<TelegramChat>>();
        services.AddTransient<ICrudRepository<Email>, CrudRepository<Email>>();
        services.AddTransient<ICrudRepository<AlertRuleNotificationGroup>, CrudRepository<AlertRuleNotificationGroup>>();
        services.AddTransient<ICrudRepository<AlertEvent>, CrudRepository<AlertEvent>>();
        services.AddTransient<ICrudRepository<AlertRule>, CrudRepository<AlertRule>>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        // services.Scan(s => s
        //     .FromCallingAssembly()
        //     .AddClasses(c => c.AssignableTo<IRepository>())
        //     .AsImplementedInterfaces()
        //     .WithTransientLifetime());
    }
}