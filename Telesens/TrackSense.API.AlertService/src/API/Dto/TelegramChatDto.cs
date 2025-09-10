using TrackSense.API.AlertService.Models.Interfaces;

namespace TrackSense.API.AlertService.Models;

public partial class TelegramChatDto : Identifiable
{
    public int ID { get; set; }
    public string TelegramChatId { get; set; }
    public int NotificationGroupId { get; set; }
}