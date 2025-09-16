using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ZipperAPI.Controllers;

namespace ZipperAPI.Tests;

public class DownloadControllerTests : IClassFixture<DownloadControllerTestSetup>
{
    private ServiceProvider _serviceProvider;
    private ProcessController _processor;
    private DownloadController _downloader;

    public DownloadControllerTests(DownloadControllerTestSetup testSetup)
    {
        _serviceProvider = testSetup.ServiceProvider;
        _processor = _serviceProvider.GetService<ProcessController>();
        _downloader = _serviceProvider.GetService<DownloadController>();
    }

    [Fact]
    public void DownloadZipAssertOkay()
    {
        //Arrange
        string[] files = { "big1.dll", "big2.dll" };

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
        var okRes = Assert.IsType<OkObjectResult>(result.Result);
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
