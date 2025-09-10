using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class AlertEvent : Identifiable
{
    public int ID { get; set; }

    public string GPS { get; set; }

    public AlertEventStatus Status { get; set; }

    public int AlertRuleId { get; set; }

    public virtual AlertRule AlertRule { get; set; }

    public long IMEI { get; set; }

    internal sealed class Configuration : IEntityTypeConfiguration<AlertEvent>
    {
        public void Configure(EntityTypeBuilder<AlertEvent> builder)
        {
            builder.ToTable("alert_events");

            builder.HasKey(m => m.ID);
            builder.Property(m => m.ID)
                .HasColumnName("id")
                .IsRequired()
                .UseIdentityColumn();
            
            builder.Property(m => m.GPS)
                .HasColumnName("gps")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasDefaultValue(AlertEventStatus.Unknown);
            
            builder.Property(m => m.AlertRuleId)
                .HasColumnName("alert_rule_id")
                .IsRequired();
            
            builder.Property(m => m.IMEI)
                .HasColumnName("imei")
                .IsRequired();

            builder.HasOne(ae => ae.AlertRule)
                .WithMany(r => r.AlertEvents)
                .HasForeignKey(ae => ae.AlertRuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(m => new {m.AlertRuleId, m.IMEI}).IsUnique();
        }
    }
}