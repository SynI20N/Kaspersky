using Microsoft.AspNetCore.Mvc;
using StatisticsAPI.Services;

namespace StatisticsAPI.Controllers;

[ApiController]
[Route("aggregate")]
public class AggregateController : ControllerBase
{
    private readonly ILogger<AggregateController> _logger;
    private readonly IAggregator _aggregator;

    public AggregateController(
        ILogger<AggregateController> logger,
        IAggregator aggregator)
    {
        _logger = logger;
        _aggregator = aggregator;
    }

    [HttpGet("group/severity")]
    public async Task GroupBySeverity(CancellationToken cancellationToken)
    {
        Response.ContentType = "text/csv";
        Response.Headers.ContentDisposition = "attachment; filename=export.csv";
        await _aggregator.AggregateBySeverityAsync(Response.Body, cancellationToken);
    }

    [HttpGet("group/product&version")]
    public async Task GroupByProductAndVersion(CancellationToken cancellationToken)
    {
        Response.ContentType = "text/csv";
        Response.Headers.ContentDisposition = "attachment; filename=export.csv";
        await _aggregator.AggregateByProductAndVersionAsync(Response.Body, cancellationToken);
    }

    [HttpGet("group/custom")]
    public async Task GroupByAllAndTimeIntervals(CancellationToken cancellationToken)
    {
        Response.ContentType = "text/csv";
        Response.Headers.ContentDisposition = "attachment; filename=export.csv";
        await _aggregator.AggregateCustom(Response.Body, cancellationToken);
    }

    [HttpGet("group/product&severity")]
    public async Task GroupByProductAndSeverity(CancellationToken cancellationToken)
    {
        Response.ContentType = "text/csv";
        Response.Headers.ContentDisposition = "attachment; filename=export.csv";
        await _aggregator.AggregateByProductAndSeverity(Response.Body, cancellationToken);
    }
}
