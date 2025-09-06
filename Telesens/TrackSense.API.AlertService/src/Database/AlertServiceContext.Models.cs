using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Npgsql;
using TrackSense.API.AlertService.Models;
using System.Data;

namespace TrackSense.API.AlertService;

public partial class AlertServiceContext : DbContext
{
    public DbSet<AlertRule> Rules => Set<AlertRule>();
    public DbSet<AlertEvent> Events => Set<AlertEvent>();
    public DbSet<NotificationGroup> Groups => Set<NotificationGroup>();
    public DbSet<TelegramChat> Telegrams => Set<TelegramChat>();
    public DbSet<Email> Emails => Set<Email>();
    public DbSet<AlertRuleNotificationGroup> GroupAlerts => Set<AlertRuleNotificationGroup>();
}