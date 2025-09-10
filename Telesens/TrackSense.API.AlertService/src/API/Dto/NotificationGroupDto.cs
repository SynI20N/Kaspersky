using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class NotificationGroupDto : Identifiable
{
    public int ID { get; set; }
    public string Name { get; set; }
}