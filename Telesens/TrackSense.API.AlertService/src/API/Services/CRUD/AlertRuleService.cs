using TrackSense.API.AlertService.Repositories;
using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService.Services.CRUD;

public class AlertRuleService : CrudService<AlertRule>
{
    public AlertRuleService(
        ILogger<AlertRuleService> logger, 
        ICrudRepository<AlertRule> ruleRepository)
        : base(logger, ruleRepository) {}
}