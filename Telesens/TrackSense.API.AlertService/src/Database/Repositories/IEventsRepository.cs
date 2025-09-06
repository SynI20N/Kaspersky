using TrackSense.API.AlertService.Models;

public interface IEventsRepository
{
    public Task<bool> InsertEventAsync(AlertEvent alertEvent);
    public Task<AlertEventStatus> GetStatusByRuleAndImeiAsync(int AlertRuleId, long Imei);
    public Task<bool> ChangeStatusByRuleAndImeiAsync(int AlertRuleId, long Imei, AlertEventStatus newStatus);
    public Task<bool> EventExistsAsync(int AlertRuleId, long Imei);
    public Task<AlertEvent?> GetByRuleAndImeiAsync(int AlertRuleId, long Imei);
}