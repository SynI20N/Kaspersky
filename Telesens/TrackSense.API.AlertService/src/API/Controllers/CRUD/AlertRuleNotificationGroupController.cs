using Microsoft.AspNetCore.Mvc;
using TrackSense.API.AlertService.Services.CRUD;
using TrackSense.API.AlertService.Models;
using AutoMapper;

namespace TrackSense.API.AlertService.Controllers.CRUD;

[ApiController]
[Route("api/rulesgroups")]
public class AlertRuleNotificationGroupController : BaseCrudController<AlertRuleNotificationGroup, AlertRuleNotificationGroupDto>
{
    public AlertRuleNotificationGroupController(ILogger<AlertRuleNotificationGroupController> logger, ICrudService<AlertRuleNotificationGroup> service, IMapper mapper)
        : base(logger, service, mapper) {}
    
    [HttpGet]
    public async Task<ActionResult<List<AlertRuleNotificationGroup>>> NotificationGroupsAsync()
    {
        return await GetEntitiesAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AlertRuleNotificationGroup>> NotificationGroupAsync(int id)
    {
        return await GetEntityAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<AlertRuleNotificationGroup>> NotificationGroupPostAsync([FromBody] AlertRuleNotificationGroupDto groupDto)
    {
        return await PostEntityAsync(groupDto);
    }

    [HttpPut]
    public async Task<ActionResult<AlertRuleNotificationGroup>> NotificationGroupPutAsync([FromBody] AlertRuleNotificationGroupDto chatDto)
    {
        return await UpdateEntityAsync(chatDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> NotificationGroupDeleteAsync(int id)
    {
        return await DeleteEntityAsync(id);
    }
}