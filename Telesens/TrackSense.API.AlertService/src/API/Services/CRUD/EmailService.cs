using TrackSense.API.AlertService.Repositories;
using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService.Services.CRUD;

public class EmailService : CrudService<Email>
{
    public EmailService(
        ILogger<EmailService> logger, 
        ICrudRepository<Email> emailRepository)
        : base(logger, emailRepository) {}
}