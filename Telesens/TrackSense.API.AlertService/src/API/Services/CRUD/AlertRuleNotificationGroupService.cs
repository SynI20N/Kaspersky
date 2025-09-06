using TrackSense.API.AlertService.Repositories;
using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService.Services.CRUD;

public class AlertRuleNotificationGroupService : CrudService<AlertRuleNotificationGroup>
{
    public AlertRuleNotificationGroupService(
        ILogger<AlertRuleNotificationGroupService> logger, 
        ICrudRepository<AlertRuleNotificationGroup> rulesGroupsService)
        : base(logger, rulesGroupsService) {}
}