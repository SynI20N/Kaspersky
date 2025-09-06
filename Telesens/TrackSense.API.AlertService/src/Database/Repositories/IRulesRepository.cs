using TrackSense.API.AlertService.Models;

public interface IRulesRepository
{
    public Task<List<AlertRule>> GetAlertRulesByImeiAsync(long imei);
}