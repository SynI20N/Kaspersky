using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TImeScaleDB.Models;

public partial class AlertEvent
{
    [Column("UniqueID")]
    [Key]
    public int ID { get; set; }

    public string GPS { get; set; }

    public AlertEventStatus Status { get; set; }

    public int AlertRuleId { get; set; }

    public AlertRule AlertRule { get; set; }

    public long IMEI { get; set; }

}