using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TImeScaleDB.Models;

public partial class AlertRuleNotificationGroup
{
    [Column("UniqueID")]
    [Key]
    public int ID { get; set; }
    public int AlertRuleId { get; set; }
    public AlertRule AlertRule { get; set; }
    public int NotificationGroupId { get; set; }
    public NotificationGroup NotificationGroup { get; set; }
}