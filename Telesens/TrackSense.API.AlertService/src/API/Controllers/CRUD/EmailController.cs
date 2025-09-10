using Microsoft.AspNetCore.Mvc;
using TrackSense.API.AlertService.Services.CRUD;
using TrackSense.API.AlertService.Models;
using AutoMapper;

namespace TrackSense.API.AlertService.Controllers.CRUD;

[ApiController]
[Route("api/emails")]
public class EmailController : BaseCrudController<Email, EmailDto>
{
    public EmailController(ILogger<EmailController> logger, ICrudService<Email> emailService, IMapper mapper)
        : base(logger, emailService, mapper) {}
    
    [HttpGet]
    public async Task<ActionResult<List<Email>>> NotificationGroupsAsync()
    {
        return await GetEntitiesAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Email>> NotificationGroupAsync(int id)
    {
        return await GetEntityAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<Email>> NotificationGroupPostAsync([FromBody] EmailDto groupDto)
    {
        return await PostEntityAsync(groupDto);
    }

    [HttpPut]
    public async Task<ActionResult<Email>> NotificationGroupPutAsync([FromBody] EmailDto chatDto)
    {
        return await UpdateEntityAsync(chatDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> NotificationGroupDeleteAsync(int id)
    {
        return await DeleteEntityAsync(id);
    }
}