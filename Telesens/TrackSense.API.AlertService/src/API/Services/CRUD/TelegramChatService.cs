using TrackSense.API.AlertService.Repositories;
using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService.Services.CRUD;

public class TelegramChatService : CrudService<TelegramChat>
{
    public TelegramChatService(
        ILogger<TelegramChatService> logger, 
        ICrudRepository<TelegramChat> telegramChatRepository)
        : base(logger, telegramChatRepository) {}
}