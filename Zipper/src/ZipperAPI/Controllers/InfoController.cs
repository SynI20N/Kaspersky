using Microsoft.AspNetCore.Mvc;
using ZipperAPI.Services;

namespace ZipperAPI.Controllers;

[ApiController]
[Route("api.zipper/v1")]
public class InfoController : ControllerBase
{
    private readonly IFolderService _folderService;
    private readonly ILogger<InfoController> _logger;

    public InfoController(IFolderService service, ILogger<InfoController> logger)
    {
        _folderService = service;
        _logger = logger;
    }

    [HttpGet("files")]
    public ActionResult<string[]> GetListOfFiles()
    {
        try
        {
            var files = _folderService.GetFiles();
            return Ok(files);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Could not get list of files: {ex.Message}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Server is misconfigured");
        }
    }
}
