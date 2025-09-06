using System.ComponentModel;

namespace TImeScaleDB.Models;

public enum AlertEventStatus
{
    [Description("The alert has just been raised")]
    Initialized = 1,
    [Description("The events linked to this alert were already fired")]
    Executed = 2,
    [Description("The alert may be falsely raised")]
    Misinterpreted = 3
}