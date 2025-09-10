using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Npgsql;
using TImeScaleDB.Models;

namespace TImeScaleDB;

public static class DbSetExtensions
{
    public static EntityEntry<T> AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
    {
        var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
        return !exists ? dbSet.Add(entity) : null;
    }
}

public class ApplicationContext : DbContext
{
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<AlertRule> Rules => Set<AlertRule>();
    public DbSet<AlertEvent> Events => Set<AlertEvent>();
    public DbSet<NotificationGroup> Groups => Set<NotificationGroup>();
    public DbSet<TelegramChat> Telegrams => Set<TelegramChat>();
    public DbSet<Email> Emails => Set<Email>();
    public DbSet<AlertRuleNotificationGroup> GroupAlerts => Set<AlertRuleNotificationGroup>();
    private string? _connString = "";

    public ApplicationContext()
    {
            var configBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfigurationRoot configuration = configBuilder.Build();
        _connString = configuration.GetConnectionString("metrics");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connString);
    }
}