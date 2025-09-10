using Microsoft.AspNetCore.Mvc;
using TrackSense.API.AlertService.Services;
using TrackSense.API.MetricGateway.Models;

namespace TrackSense.API.AlertService.Controllers;

[ApiController]
[Route("api")]
public class AnalyzerController : ControllerBase
{
    private readonly ILogger<AnalyzerController> _logger;
    private readonly IMetricAnalyzerService _analyzer;
 
    public AnalyzerController(ILogger<AnalyzerController> logger, IMetricAnalyzerService analyzer)
    {
        _logger = logger;
        _analyzer = analyzer;
    }

    [HttpPost("alalyze")]
    public async Task<ActionResult> MetricAnalyzeAsync([FromBody] RawMetric metric)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        await _analyzer.MetricAnalyzeAsync(metric);

        return Ok("metric feed analyzed!");
    }
}