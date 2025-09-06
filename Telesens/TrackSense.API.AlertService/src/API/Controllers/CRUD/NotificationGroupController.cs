using Microsoft.AspNetCore.Mvc;
using TrackSense.API.AlertService.Services.CRUD;
using TrackSense.API.AlertService.Models;
using AutoMapper;

namespace TrackSense.API.AlertService.Controllers.CRUD;

[ApiController]
[Route("api/groups")]
public class NotificationGroupController : BaseCrudController<NotificationGroup, NotificationGroupDto>
{
    public NotificationGroupController(ILogger<NotificationGroupController> logger, ICrudService<NotificationGroup> notificationGroupService, IMapper mapper)
        : base(logger, notificationGroupService, mapper) {}
    
    [HttpGet]
    public async Task<ActionResult<List<NotificationGroup>>> NotificationGroupsAsync()
    {
        return await GetEntitiesAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<NotificationGroup>> NotificationGroupAsync(int id)
    {
        return await GetEntityAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<NotificationGroup>> NotificationGroupPostAsync([FromBody] NotificationGroupDto groupDto)
    {
        return await PostEntityAsync(groupDto);
    }

    [HttpPut]
    public async Task<ActionResult<NotificationGroup>> NotificationGroupPutAsync([FromBody] NotificationGroupDto chatDto)
    {
        return await UpdateEntityAsync(chatDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> NotificationGroupDeleteAsync(int id)
    {
        return await DeleteEntityAsync(id);
    }
}