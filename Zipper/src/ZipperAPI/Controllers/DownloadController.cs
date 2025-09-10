using Microsoft.AspNetCore.Mvc;
using ZipperAPI.Services;

namespace ZipperAPI.Controllers;

[ApiController]
[Route("zipper")]
public class DownloadController : ControllerBase
{
    private readonly ILogger<DownloadController> _logger;
    private readonly IProcessHandler _processHandler;

    public DownloadController(ILogger<DownloadController> logger, IProcessHandler processHandler)
    {
        _logger = logger;
        _processHandler = processHandler;
    }

    [HttpGet("archive/download/{id}")]
    public IActionResult Download(int id)
    {
        try
        {
            Stream stream = _processHandler.ProcessStream(id);
            return File(stream, "application/octet-stream", "files.zip");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"User tried to check inexistent or in-progress process: {ex.Message}");
        }
        return NotFound();
    }
}
