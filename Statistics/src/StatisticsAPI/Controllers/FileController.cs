using Microsoft.AspNetCore.Mvc;
using StatisticsAPI.Dto;
using StatisticsAPI.Models;
using StatisticsAPI.Services;

namespace StatisticsAPI.Controllers;

[ApiController]
[Route("csv")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;
    private readonly IFilePersistence _fileService;
    private readonly long _maxFileSize;

    public FileController(IConfiguration configuration, ILogger<FileController> logger, IFilePersistence filePersistence)
    {
        _maxFileSize = configuration.GetSection("Storage").GetValue<long>("MaxAllowedSize");
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
    public async Task<IActionResult> UploadAsync(IFormFile file, CancellationToken token)
    {
        if (file == null) { return BadRequest("file empty"); }
        if (file.Length == 0 || file.Length > _maxFileSize)
        {
            _logger.LogWarning("Uploaded incorrent file");

            return BadRequest("wrong file size");
        }
        using (Stream s = file.OpenReadStream())
        {
            try
            {
                await _fileService.Validate(s, token);
            }
            catch (FormatException)
            {
                _logger.LogWarning("incorrect file format");

                return BadRequest("wrong file format");
            }
            if (s.CanSeek) s.Seek(0, SeekOrigin.Begin);
            else
            {
                _logger.LogError("Could not seek a stream of input file");

                return StatusCode(500);
            }

            string id = await _fileService.UploadAsync(s, token);
            string app_path = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            FileMetadataDto meta = new FileMetadataDto(id, file.Name, file.Length, app_path + $"download/{id}");
            return Ok(meta);
        }
    }

    /// <summary>
    /// Download the file by id
    /// </summary>
    /// <param name="id">Id of a file in storage</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Handle for download</returns>
    [HttpGet("download/{id}")]
    public async Task<IActionResult> Download(string id, CancellationToken cancellationToken)
    {
        Response.ContentType = "text/csv";
        Response.Headers.ContentDisposition = "attachment; filename=export.csv";
        await _fileService.Download(id, Response.Body, cancellationToken);

        return Ok("File downloaded");
    }
}
