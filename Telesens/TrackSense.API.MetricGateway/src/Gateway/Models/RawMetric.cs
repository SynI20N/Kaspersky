using System.Text.Json.Serialization;

namespace TrackSense.API.MetricGateway.Models
{
    public class RawMetric
    {
        public long Imei { get; init; }

        [JsonPropertyName("DateTime")]
        public DateTime DateTime { get; init; }

        [JsonPropertyName("GPS")]
        public string GPS { get; init; }

        [JsonPropertyName("Sensor1")]
        public float Sensor1 { get; init; }

        [JsonPropertyName("Sensor2")]
        public float Sensor2 { get; init; }

        [JsonPropertyName("Sensor3")]
        public float Sensor3 { get; init; }

        [JsonPropertyName("G_sensor")]
        public float G_sensor { get; init; }

        [JsonPropertyName("Volt")]
        public float Volt { get; init; }

        [JsonPropertyName("Temperature")]
        public short Temperature { get; init; }

        [JsonPropertyName("Humidity")]
        public byte Humidity { get; init; }

        [JsonPropertyName("Barometer")]
        public byte Barometer { get; init; }
    }
}
