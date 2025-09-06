namespace TrackSense.API.AlertService.Workers;

internal class AlertService : BackgroundService
{
    private readonly ILogger<AlertService> _logger;
    private readonly IConfiguration _configuration;
    public AlertService(ILogger<AlertService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        _logger.LogDebug("Path: {path} interval: {int}", _configuration["Paths:NotifyAPI"], _configuration["Interval"]);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            try
            {
                var client = new HttpClient();

                var webRequest = new HttpRequestMessage(HttpMethod.Post, _configuration["Paths:NotifyAPI"]);

                var response = client.Send(webRequest);

                using var reader = new StreamReader(response.Content.ReadAsStream());

                _logger.LogInformation("Request result: {result}", reader.ReadToEnd());
            }
            catch (Exception e)
            {
                _logger.LogError("Error notifying clients: {error} innerException: {innerException}", e.Message, e.InnerException);
            }
            await Task.Delay(int.Parse(_configuration["Intervals:NotificationWorker"]) * 1000, stoppingToken);
        }
    }
}
