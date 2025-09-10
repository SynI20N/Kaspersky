using Confluent.Kafka;
using System.Text.Json.Serialization;
using System.Text.Json;
using TrackSense.API.MetricGateway.Models;

namespace TrackSense.API.MetricGateway.Services;

public class KafkaSender : IKafkaSender
{
    private readonly string _broker = Environment.GetEnvironmentVariable("BROKER");
    private readonly string _topic = "MainBus";
    private readonly ILogger<KafkaSender> _logger;
    private readonly IConfiguration _configuration;

    public KafkaSender(ILogger<KafkaSender> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _topic = _configuration["Kafka:Topic"];
    }


    public async Task ProduceAsync(RawMetric metric)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _broker,
            Acks = Acks.All
        };

        using var producer = new ProducerBuilder<long, string>(config).Build();

        var message = new Message<long, string>
        {
            Key = metric.Imei,
            Value = JsonSerializer.Serialize(metric)
        };
        
        try{
            var deliveryResult = await producer.ProduceAsync(_topic, message);
        }
        catch(ProduceException<long, string> e)
        {
            _logger.LogError(e.Message);
            return;
        }
            

        producer.Flush(TimeSpan.FromSeconds(10));

        _logger.LogInformation($"Sent data to Kafka: {message.Value}");
    }
}

public sealed class JsonValueSerializer<T> : ISerializer<T>, IDeserializer<T>
{
private static readonly JsonSerializerOptions _serializerOptions;

static JsonValueSerializer()
{
    _serializerOptions = new JsonSerializerOptions();
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
}

public byte[] Serialize(T data, SerializationContext context) => JsonSerializer.SerializeToUtf8Bytes(data, _serializerOptions);

public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
{
    if (isNull)
        throw new ArgumentNullException(nameof(data), "Null data encountered");

    return JsonSerializer.Deserialize<T>(data, _serializerOptions) ??
           throw new ArgumentNullException(nameof(data), "Null data encountered");
}
}