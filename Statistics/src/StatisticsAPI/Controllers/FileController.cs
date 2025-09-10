using Microsoft.AspNetCore.Mvc;
using StatisticsAPI.Dto;
using StatisticsAPI.Services;

namespace StatisticsAPI.Controllers;

[ApiController]
[Route("csv")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;
    private readonly IFilePersistence _fileService;

    public FileController(ILogger<FileController> logger, IFilePersistence filePersistence)
    {
        _logger = logger;
        _fileService = filePersistence;
    }

    /// <summary>
    /// Upload file from remote
    /// </summary>
    /// <param name="file">Http file</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Metadata of uploaded file</returns>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(FileMetadataDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadAsync(IFormFile file, CancellationToken token)
    {
        try
        {
            string id = await _fileService.UploadAsync(file, token);
            string app_path = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            FileMetadataDto meta = new FileMetadataDto(id, file.Name, file.Length, app_path + $"/download/{id}");
            return Ok(meta);
        }
        catch (FormatException f)
        {
            return BadRequest(f.Message);
        }
        catch (ArgumentException a)
        {
            return BadRequest(a.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Download the file by id
    /// </summary>
    /// <param name="id">Id of a file in storage</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Handle for download</returns>
    [HttpGet("download/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task Download(string id, CancellationToken cancellationToken)
    {
        Response.ContentType = "text/csv";
        Response.Headers.ContentDisposition = "attachment; filename=export.csv";
        await _fileService.DownloadAsync(id, Response.Body, cancellationToken);
    }
}
