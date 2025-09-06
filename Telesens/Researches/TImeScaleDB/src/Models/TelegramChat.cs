using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TImeScaleDB.Models;

public partial class TelegramChat
{
    [Column("UniqueID")]
    [Key]
    public int ID { get; set; }
    public string TelegramChatId { get; set; }
    public int NotificationGroupId { get; set; }
    public NotificationGroup NotificationGroup { get; set; }
}