using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class NotificationGroup : Identifiable
{
    public int ID { get; set; }
    public string Name { get; set; }
    public virtual List<TelegramChat> Telegrams { get; set; } = new List<TelegramChat>();
    public virtual List<Email> Emails { get; set; } = new List<Email>();
    public virtual List<AlertRuleNotificationGroup> AlertRuleNotificationGroups { get; set; } = new List<AlertRuleNotificationGroup>();

    internal sealed class Configuration : IEntityTypeConfiguration<NotificationGroup>
    {
        public void Configure(EntityTypeBuilder<NotificationGroup> builder)
        {
            builder.ToTable("notification_groups");

            builder.HasKey(m => m.ID);
            builder.Property(m => m.ID)
                .HasColumnName("id")
                .IsRequired()
                .UseIdentityColumn();
            
            builder.Property(m => m.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}