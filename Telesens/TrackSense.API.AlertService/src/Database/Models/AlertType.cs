using System.ComponentModel;

namespace TrackSense.API.AlertService.Models;

public enum AlertType
{
    [Description("Basic info for logging")]
    Information = 1,
    [Description("Some measures could be taken")]
    Warning = 2,
    [Description("The issue must be resolved")]
    Critical = 3
}