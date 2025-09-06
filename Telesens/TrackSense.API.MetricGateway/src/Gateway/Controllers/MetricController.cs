using Microsoft.AspNetCore.Mvc;
using TrackSense.API.MetricGateway.Models;
using TrackSense.API.MetricGateway.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TrackSense.API.MetricGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetricController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MetricController> _logger;
    private readonly IKafkaSender _kafkaSender;
    private readonly ByteJsonConverter _byteJsonConverter;
    private readonly IConfiguration _configuration;

    public MetricController(
        HttpClient httpClient, 
        ILogger<MetricController> logger, 
        IKafkaSender kafkaSender, 
        ByteJsonConverter byteJsonConverter,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _kafkaSender = kafkaSender;
        _byteJsonConverter = byteJsonConverter;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> GetSensorData()
    {
        /*
            Считывание данных с эмулятора датчика (http://localhost:8000/sensor-data)
        */
        try
        {
            var response = await _httpClient.GetAsync(_configuration["MetricFeedUri"]);
            var jsonData = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching sensor data: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, "Error fetching sensor data");
            }
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(), _byteJsonConverter },
                PropertyNameCaseInsensitive = true
            };

            var sensorData = JsonSerializer.Deserialize<RawMetric>(jsonData, options);
            _logger.LogInformation($"Received sensor data:\n{jsonData}");

            await _kafkaSender.ProduceAsync(sensorData);

            return Ok(sensorData);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e.Message);
            return BadRequest($"Could not fetch metrics on remote address : {e.Message}");
        }
        
    }

    [HttpPost(Name = "metric")]
    public async Task<IActionResult> PostMetric([FromBody] RawMetric metric)
    {
        await _kafkaSender.ProduceAsync(metric);
        return Ok();
    }
}