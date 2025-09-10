using Microsoft.AspNetCore.Mvc;
using TrackSense.API.AlertService.Services.CRUD;
using TrackSense.API.AlertService.Models;
using AutoMapper;

namespace TrackSense.API.AlertService.Controllers.CRUD;

[ApiController]
[Route("api/rules")]
public class AlertRuleController : BaseCrudController<AlertRule, AlertRuleDto>
{
    public AlertRuleController(ILogger<AlertRuleController> logger, ICrudService<AlertRule> alertRuleService, IMapper mapper)
        : base(logger, alertRuleService, mapper) {}
    
    [HttpGet]
    public async Task<ActionResult<List<AlertRule>>> NotificationGroupsAsync()
    {
        return await GetEntitiesAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AlertRule>> NotificationGroupAsync(int id)
    {
        return await GetEntityAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<AlertRule>> NotificationGroupPostAsync([FromBody] AlertRuleDto groupDto)
    {
        return await PostEntityAsync(groupDto);
    }

    [HttpPut]
    public async Task<ActionResult<AlertRule>> NotificationGroupPutAsync([FromBody] AlertRuleDto chatDto)
    {
        return await UpdateEntityAsync(chatDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> NotificationGroupDeleteAsync(int id)
    {
        return await DeleteEntityAsync(id);
    }
}