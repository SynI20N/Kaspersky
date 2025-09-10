using Microsoft.AspNetCore.Mvc;
using ZipperAPI.Services;

namespace ZipperAPI.Controllers;

[ApiController]
[Route("zipper")]
public class InfoController : ControllerBase
{
    private readonly IFolderService _folderService;

    public InfoController(IFolderService service)
    {
        _folderService = service;
    }

    [HttpGet("files")]
    public ActionResult<List<string>> GetListOfFiles()
    {
        var absolutePath = _folderService.GetWorkingDir();
        var files = Directory.GetFiles(absolutePath)
                             .Select(Path.GetFileName) 
                             .ToList();

        return Ok(files);
    }
}
