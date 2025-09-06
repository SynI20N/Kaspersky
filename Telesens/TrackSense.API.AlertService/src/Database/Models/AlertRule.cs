using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class AlertRule : Identifiable
{
    public int ID { get; set; }

    public AlertType Type { get; set; }

    public float CriticalValue { get; set; }

    public Operator Operator { get; set; }

    public string ValueName { get; set; }

    public long Imei { get; set; }

    public virtual List<AlertEvent> AlertEvents { get; set; } = new List<AlertEvent>();
    public virtual List<AlertRuleNotificationGroup> AlertRuleNotificationGroups { get; set; } = new List<AlertRuleNotificationGroup>();

    internal sealed class Configuration : IEntityTypeConfiguration<AlertRule>
    {
        public void Configure(EntityTypeBuilder<AlertRule> builder)
        {
            builder.ToTable("alert_rules");

            builder.HasKey(m => m.ID);
            builder.Property(m => m.ID)
                .HasColumnName("id")
                .IsRequired()
                .UseIdentityColumn();
            
            builder.Property(m => m.Type)
                .HasColumnName("type")
                .IsRequired();

            builder.Property(m => m.CriticalValue)
                .HasColumnName("critical_value")
                .IsRequired();
            
            builder.Property(m => m.Operator)
                .HasColumnName("operator")
                .IsRequired();
            
            builder.Property(m => m.ValueName)
                .HasColumnName("value_name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Imei)
                .HasColumnName("imei")
                .IsRequired();
        }
    }
}