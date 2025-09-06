using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TImeScaleDB.Models;

public partial class AlertRule
{
    [Column("UniqueID")]
    [Key]
    public int ID { get; set; }

    public AlertType Type { get; set; }

    public float CriticalValue { get; set; }

    public Operator Operator { get; set; }

    public string ValueName { get; set; }

    //IMEA
}