using Microsoft.AspNetCore.Mvc;
using ZipperAPI.Services;

namespace ZipperAPI.Controllers;

[ApiController]
[Route("api.zipper/v1")]
public class ProcessController : ControllerBase
{
    private readonly IProcessHandler _processHandler;
    private readonly ILogger<IProcessHandler> _logger;
    private readonly IFolderService _folderService;

    public ProcessController(IProcessHandler processHandler, ILogger<IProcessHandler> logger, IFolderService service)
    {
        _processHandler = processHandler;
        _logger = logger;
        _folderService = service;
    }

    [HttpPost("archive/start")]
    public ActionResult<string> ArchiveFiles([FromBody] string[] files)
    {
        if (files == null || files.Length == 0)
        {
            _logger.LogWarning("User provided empty file names");
            return BadRequest("Files are empty");
        }

        var folderPath = _folderService.GetWorkingDir();

        var missingFiles = files
            .Where(f => !System.IO.File.Exists(Path.Combine(folderPath, f)))
            .ToList();

        if (missingFiles.Any())
        {
            _logger.LogError("User requested archiving with missing files: {Files}", string.Join(", ", missingFiles));
            return BadRequest($"Some files do not exist: {string.Join(", ", missingFiles)}");
        }

        try
        {
            int processId = _processHandler.StartArchiveProcess(files);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError("Server paths are not configures: ", e.Message);
            return StatusCodes.Status503ServiceUnavailable;
        }

        return Accepted(processId.ToString());
    }


    [HttpGet("archive/status/{id}")]
    public ActionResult<string> GetArchiveStatus(int id)
    {
        try
        {
            return Ok(_processHandler.ProcessStatus(id).ToString());
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"User tried to check inexistent process: {ex.Message}");
            return NotFound();
        }
    }
}
