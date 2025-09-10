using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TImeScaleDB.Models;

public partial class Sensor
{
      [Key]
      public DateTime UTC { get; set; }

      public string GPS { get; set; }

      public float sensor1 { get; set; }

      public float sensor2 { get; set; }
      
      public float sensor3 { get; set; }

      public float g_sensor { get; set; }

      public float volt { get; set; }

      public ushort temp { get; set; }

      public byte hum { get; set; }

      public float barometer { get; set; }
}
