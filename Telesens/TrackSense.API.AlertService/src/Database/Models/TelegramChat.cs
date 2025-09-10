using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class TelegramChat : Identifiable
{
    public int ID { get; set; }
    public string TelegramChatId { get; set; }
    public int NotificationGroupId { get; set; }
    public virtual NotificationGroup NotificationGroup { get; set; }

    internal sealed class Configuration : IEntityTypeConfiguration<TelegramChat>
    {
        public void Configure(EntityTypeBuilder<TelegramChat> builder)
        {
            builder.ToTable("telegram_chats");

            builder.HasKey(m => m.ID);
            builder.Property(m => m.ID)
                .HasColumnName("id")
                .IsRequired()
                .UseIdentityColumn();
            
            builder.Property(m => m.TelegramChatId)
                .HasColumnName("telegram_chat_id")
                .IsRequired();

            builder.Property(m => m.NotificationGroupId)
                .HasColumnName("notification_group_id")
                .IsRequired();

            builder.HasOne(e => e.NotificationGroup)
                .WithMany(n => n.Telegrams)
                .HasForeignKey(e => e.NotificationGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}