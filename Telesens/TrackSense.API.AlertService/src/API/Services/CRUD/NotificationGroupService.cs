using TrackSense.API.AlertService.Repositories;
using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService.Services.CRUD;

public class NotificationGroupService : CrudService<NotificationGroup>
{
    public NotificationGroupService(
        ILogger<NotificationGroupService> logger, 
        ICrudRepository<NotificationGroup> notificationGroupRepository)
        : base(logger, notificationGroupRepository) {}
}