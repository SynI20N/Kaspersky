using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TImeScaleDB.Models;

public partial class NotificationGroup
{
    [Column("UniqueID")]
    [Key]
    public int ID { get; set; }
    public string Name { get; set; }
    public List<TelegramChat> Telegrams { get; set; } = new List<TelegramChat>();
    public List<Email> Emails { get; set; } = new List<Email>();
}