using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class AlertRuleNotificationGroupDto : Identifiable
{
    public int ID { get; set; }
    public int AlertRuleId { get; set; }
    public int NotificationGroupId { get; set; }
}