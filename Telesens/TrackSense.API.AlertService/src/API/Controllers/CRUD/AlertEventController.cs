using Microsoft.AspNetCore.Mvc;
using TrackSense.API.AlertService.Services.CRUD;
using TrackSense.API.AlertService.Models;
using AutoMapper;

namespace TrackSense.API.AlertService.Controllers.CRUD;

[ApiController]
[Route("api/events")]
public class AlertEventController : BaseCrudController<AlertEvent, AlertEventDto>
{
    public AlertEventController(ILogger<AlertRuleController> logger, ICrudService<AlertEvent> alertEventService, IMapper mapper)
        : base(logger, alertEventService, mapper) {}
    
    [HttpGet]
    public async Task<ActionResult<List<AlertEvent>>> NotificationGroupsAsync()
    {
        return await GetEntitiesAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AlertEvent>> NotificationGroupAsync(int id)
    {
        return await GetEntityAsync(id);
    }
}