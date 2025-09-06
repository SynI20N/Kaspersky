using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using TrackSense.API.AlertService.Workers.Services;
using TrackSense.API.MetricGateway.Models;

namespace TrackSense.API.AlertService.Workers;

internal class AlertDataCrawler : BackgroundService
{
    private readonly ILogger<AlertDataCrawler> _logger;
    private readonly IConfiguration _configuration;
    private readonly IKafkaReceiver _kafkaReceiver;
    public AlertDataCrawler(ILogger<AlertDataCrawler> logger, IConfiguration configuration, IKafkaReceiver kafkaReceiver)
    {
        _logger = logger;
        _configuration = configuration;
        _kafkaReceiver = kafkaReceiver;

        _logger.LogDebug("Starting to receive messages from Kafka");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            try
            {
                RawMetric metric = _kafkaReceiver.Receive(stoppingToken);

                _logger.LogInformation($"Received metric : {metric}");

                if (metric != null)
                {
                    var client = new HttpClient();

                    var webRequest = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(_configuration["Paths:KafkaMetricFeedAPI"]),
                        Content = new StringContent(
                            JsonConvert.SerializeObject(metric),
                            Encoding.UTF8,
                            MediaTypeNames.Application.Json),
                    };

                    var response = client.Send(webRequest);

                    using var reader = new StreamReader(response.Content.ReadAsStream());

                    _logger.LogInformation("Send metric to API result: {result}", reader.ReadToEnd());
                }

            }
            catch (Exception e)
            {
                _logger.LogError("Error sending metric to API: {error}, innerException: {innerException}", e.Message, e.InnerException);
            }
            await Task.Delay(int.Parse(_configuration["Intervals:KafkaFeedWorker"]) * 1000, stoppingToken);
        }
    }
}
