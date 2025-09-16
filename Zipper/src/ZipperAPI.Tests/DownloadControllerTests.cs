using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ZipperAPI.Controllers;
using ZipperAPI.Services;

namespace ZipperAPI.Tests;

[Collection("ControllerTests")]
public class DownloadControllerTests : IClassFixture<ControllerTestSetup>
{
    private ServiceProvider _serviceProvider;
    private ProcessController _processor;
    private DownloadController _downloader;

    public DownloadControllerTests(ControllerTestSetup testSetup)
    {
        _serviceProvider = testSetup.ServiceProvider;
        _processor = _serviceProvider.GetService<ProcessController>();
        _downloader = _serviceProvider.GetService<DownloadController>();
    }

    [Fact]
    public void DownloadZipAssertOkay()
    {
        //Arrange
        string[] files = { "long.txt", "big2.dll" };

        //Act
        ActionResult<string> result = _processor.ArchiveFiles(files);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Int32.Parse(Assert.IsAssignableFrom<string>(okResult.Value));
        ActionResult<string> status = _processor.GetArchiveStatus(response);

        var ok = Assert.IsType<OkObjectResult>(status.Result);
        var statusString = Assert.IsAssignableFrom<string>(ok.Value);

        while (statusString == "InProgress")
        {
            ActionResult<string> s = _processor.GetArchiveStatus(response);
            var o = Assert.IsType<OkObjectResult>(s.Result);
            statusString = Assert.IsAssignableFrom<string>(o.Value);
        }
        Assert.Equal("Completed", statusString);

        var res = _downloader.Download(response);

        //Assert
        var okDownload = Assert.IsType<FileStreamResult>(res);
        var resp = Assert.IsAssignableFrom<FileStreamResult>(res);
        resp.FileStream.Close();
    }

    [Fact]
    public void DownloadInexistentAssertNotFound()
    {
        //Arrange
        int processId = -1314;

        //Act
        IActionResult res = _downloader.Download(processId);

        //Assert
        var okResult = Assert.IsType<NotFoundResult>(res);
    }
}
