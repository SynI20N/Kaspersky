using Microsoft.AspNetCore.Mvc;
using TrackSense.API.AlertService.Services.CRUD;
using TrackSense.API.AlertService.Models;
using AutoMapper;

namespace TrackSense.API.AlertService.Controllers.CRUD;

[ApiController]
[Route("api/chats")]
public class TelegramChatController : BaseCrudController<TelegramChat, TelegramChatDto>
{
    public TelegramChatController(ILogger<TelegramChatController> logger, ICrudService<TelegramChat> telegramChatService, IMapper mapper)
        : base(logger, telegramChatService, mapper) {}

    [HttpGet]
    public async Task<ActionResult<List<TelegramChat>>> TelegramChatsAsync()
    {
        return await GetEntitiesAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TelegramChat>> TelegramChatAsync(int id)
    {
        return await GetEntityAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<TelegramChat>> TelegramChatPostAsync([FromBody] TelegramChatDto chatDto)
    {
        return await PostEntityAsync(chatDto);
    }

    [HttpPut]
    public async Task<ActionResult<TelegramChat>> TelegramChatUpdateAsync([FromBody] TelegramChatDto chatDto)
    {
        return await UpdateEntityAsync(chatDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> TelegramChatDeleteAsync(int id)
    {
        return await DeleteEntityAsync(id); 
    }
}