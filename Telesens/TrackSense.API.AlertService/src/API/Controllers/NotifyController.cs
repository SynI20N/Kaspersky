using Microsoft.AspNetCore.Mvc;
using TrackSense.API.AlertService.Services;

namespace TrackSense.API.AlertService.Controllers;

[ApiController]
[Route("api")]
public class NotifyController : ControllerBase
{
    private readonly ILogger<NotifyController> _logger;
    private readonly INotificationService _notifications;
 
    public NotifyController(ILogger<NotifyController> logger, INotificationService notifications)
    {
        _logger = logger;
        _notifications = notifications;
    }

    [HttpPost("notify")]
    public async Task<ActionResult> NotifyAsync()
    {
        await _notifications.NotifyAsync();

        return Ok("all clients notified!");
    }
}