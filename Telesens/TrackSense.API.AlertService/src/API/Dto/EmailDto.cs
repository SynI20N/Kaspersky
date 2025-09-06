using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class EmailDto : Identifiable
{
    public int ID { get; set; }
    public string EmailValue { get; set; }
    public int NotificationGroupId { get; set; }
}