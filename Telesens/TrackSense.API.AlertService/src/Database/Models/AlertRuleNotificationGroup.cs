using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class AlertRuleNotificationGroup : Identifiable
{
    public int ID { get; set; }
    public int AlertRuleId { get; set; }
    public virtual AlertRule AlertRule { get; set; }
    public int NotificationGroupId { get; set; }
    public virtual NotificationGroup NotificationGroup { get; set; }

    internal sealed class Configuration : IEntityTypeConfiguration<AlertRuleNotificationGroup>
    {
        public void Configure(EntityTypeBuilder<AlertRuleNotificationGroup> builder)
        {
            builder.ToTable("alert_rules_notification_groups");

            builder.HasKey(m => m.ID);
            builder.Property(m => m.ID)
                .HasColumnName("id")
                .IsRequired()
                .UseIdentityColumn();
            
            builder.Property(m => m.AlertRuleId)
                .HasColumnName("alert_rule_id")
                .IsRequired();

            builder.Property(m => m.NotificationGroupId)
                .HasColumnName("notification_group_id")
                .IsRequired();

            builder.HasOne(aeng => aeng.AlertRule)
                .WithMany(r => r.AlertRuleNotificationGroups)
                .HasForeignKey(aeng => aeng.AlertRuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(aeng => aeng.NotificationGroup)
                .WithMany(r => r.AlertRuleNotificationGroups)
                .HasForeignKey(aeng => aeng.NotificationGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}