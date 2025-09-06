using TrackSense.API.AlertService.Repositories;
using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService.Services.CRUD;

public class AlertEventService : CrudService<AlertEvent>
{
    public AlertEventService(
        ILogger<AlertEventService> logger, 
        ICrudRepository<AlertEvent> eventRepository)
        : base(logger, eventRepository) {}
}