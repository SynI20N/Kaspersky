using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class AlertEventDto : Identifiable
{
    public int ID { get; set; }

    public string GPS { get; set; }

    public AlertEventStatus Status { get; set; }

    public int AlertRuleId { get; set; }

    public long IMEI { get; set; }
}