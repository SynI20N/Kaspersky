using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ZipperAPI.Controllers;
using ZipperAPI.Services;

namespace ZipperAPI.Tests;

[Collection("ControllerTests")]
public class InfoControllerTests : IClassFixture<ControllerTestSetup>
{
    private ServiceProvider _serviceProvider;
    private InfoController _controller;

    public InfoControllerTests(ControllerTestSetup testSetup)
    {
        _serviceProvider = testSetup.ServiceProvider;
        _controller = _serviceProvider.GetService<InfoController>();
    }

    [Fact]
    public void GetFileNamesShouldResultAll()
    {
        //Arrange
        var expected = new List<string> { "200mb.csv", "big1.dll", "big2.dll", "empty1.txt", "empty2.txt" };

        //Act
        ActionResult<string[]> result = _controller.GetListOfFiles();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var files = Assert.IsAssignableFrom <string[]>(okResult.Value);

        Assert.Equal(expected, files);
    }
}
