using System.ComponentModel;

namespace TrackSense.API.AlertService.Models;

public enum AlertEventStatus
{
    [Description("Default fallback")]
    Unknown = 1,
    [Description("Created but have not been alerted yet")]
    Pending = 2,
    [Description("Alerted and still active")]
    Active = 3,
    [Description("Been alerted but came back to normal")]
    Deactivate = 4,
    [Description("Alert has not reached target")]
    NotNotified = 5
}