using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ZipperAPI.Controllers;
using ZipperAPI.Services;

namespace ZipperAPI.Tests;

[Collection("ControllerTests")]
public class ProcessControllerTests : IClassFixture<ControllerTestSetup>
{
    private ServiceProvider _serviceProvider;
    private ProcessController _controller;

    public ProcessControllerTests(ControllerTestSetup testSetup)
    {
        _serviceProvider = testSetup.ServiceProvider;
        _controller = _serviceProvider.GetService<ProcessController>();
    }

    [Fact]
    public void GetProcessStatusShouldReturnNotFail()
    {
        //Arrange
        string[] files = { "big1.dll", "big2.dll" };

        //Act
        ActionResult<string> result = _controller.ArchiveFiles(files);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Int32.Parse(Assert.IsAssignableFrom<string>(okResult.Value));
        ActionResult<string> status = _controller.GetArchiveStatus(response);

        var ok = Assert.IsType<OkObjectResult>(status.Result);
        var statusString = Assert.IsAssignableFrom<string>(ok.Value);

        //Assert
        while (statusString == "InProgress")
        {
            ActionResult<string> s = _controller.GetArchiveStatus(response);
            var o = Assert.IsType<OkObjectResult>(s.Result);
            statusString = Assert.IsAssignableFrom<string>(o.Value);
        }
        Assert.Equal("Completed", statusString);
    }

    [Fact]
    public void GetProcessStatusAssertFailed()
    {
        //Arrange
        string[] files = { "haha.txt", "big2.dll" };

        //Act
        ActionResult<string> result = _controller.ArchiveFiles(files);

        //Assert
        var res = Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void DownloadFailedZipAssertNotFound()
    {
        //Arrange
        string[] files = { "big1.dll", "big2.dll" };
        string[] files_wrong = { "haha", "big2.dll" };
        string[] files_ok = { "big1.dll", "big2.dll" };

        //Act
        ActionResult<string> r = _controller.ArchiveFiles(files);
        ActionResult<string> r2 = _controller.ArchiveFiles(files_wrong);
        ActionResult<string> r3 = _controller.ArchiveFiles(files_ok);

        var okResult = Assert.IsType<BadRequestObjectResult>(r2.Result);
        string response = Assert.IsAssignableFrom<string>(okResult.Value);

        //Assert
        Assert.Equal("Some files do not exist: haha", response);
    }

    [Fact]
    public void LongJobResponsiveness()
    {
        //Arrange
        string[] files = { "big1.dll", "big2.dll", "200mb.csv" };

        //Act
        ActionResult<string> result = _controller.ArchiveFiles(files);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Int32.Parse(Assert.IsAssignableFrom<string>(okResult.Value));
        ActionResult<string> status = _controller.GetArchiveStatus(response);

        var ok = Assert.IsType<OkObjectResult>(status.Result);
        var statusString = Assert.IsAssignableFrom<string>(ok.Value);

        //Assert
        while (statusString == "InProgress")
        {
            ActionResult<string> s = _controller.GetArchiveStatus(response);
            var o = Assert.IsType<OkObjectResult>(s.Result);
            statusString = Assert.IsAssignableFrom<string>(o.Value);
        }
        Assert.Equal("Completed", statusString);
    }
}
