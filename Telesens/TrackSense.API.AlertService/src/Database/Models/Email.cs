using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class Email : Identifiable
{
    public int ID { get; set; }
    public string EmailValue { get; set; }
    public int NotificationGroupId { get; set; }
    public virtual NotificationGroup NotificationGroup { get; set; }

    internal sealed class Configuration : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.ToTable("emails");

            builder.HasKey(m => m.ID);
            builder.Property(m => m.ID)
                .HasColumnName("id")
                .IsRequired()
                .UseIdentityColumn();
            
            builder.Property(m => m.EmailValue)
                .HasColumnName("email_value")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.NotificationGroupId)
                .HasColumnName("notification_group_id")
                .IsRequired();

            builder.HasOne(e => e.NotificationGroup)
                .WithMany(n => n.Emails)
                .HasForeignKey(e => e.NotificationGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}