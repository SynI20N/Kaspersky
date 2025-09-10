using System.Text.RegularExpressions;
using Confluent.Kafka;
using Newtonsoft.Json;
using TrackSense.API.MetricGateway.Models;

namespace TrackSense.API.AlertService.Workers.Services;

public class KafkaReceiver : IKafkaReceiver, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<KafkaReceiver> _logger;
    private readonly string _broker = Environment.GetEnvironmentVariable("BROKER");
    private readonly string _topic; 

    private IConsumer<long, string> _consumer;

    public KafkaReceiver(IConfiguration configuration, ILogger<KafkaReceiver> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _topic = _configuration["Kafka:Topic"];
    }

    public RawMetric Receive(CancellationToken ct)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _broker,
            GroupId = "alert-consumer",
        };

        RawMetric metric = null;

        if(_consumer == null)
        {
            _consumer = new ConsumerBuilder<long, string>(config).Build();
            _consumer.Subscribe(_topic);
        }

        try
        {
            var cr = _consumer.Consume(ct);
            Console.WriteLine($"Consumed metric from topic {_topic}: key = {cr.Message.Key,-10} value = {cr.Message.Value}");
            metric = JsonConvert.DeserializeObject<RawMetric>(cr.Message.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }

        return metric;
    }

    public void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
    }
}