using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class AlertRuleDto : Identifiable
{
    public int ID { get; set; }

    public AlertType Type { get; set; }

    public float CriticalValue { get; set; }

    public Operator Operator { get; set; }

    public string ValueName { get; set; }
    
    public long Imei { get; set; }
}