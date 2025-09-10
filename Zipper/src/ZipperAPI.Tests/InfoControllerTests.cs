using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZipperAPI.Controllers;

namespace ZipperAPI.Tests;

public class InfoControllerTests : IClassFixture<InfoControllerTestSetup>
{
    private ServiceProvider _serviceProvider;
    private InfoController _controller;

    public InfoControllerTests(InfoControllerTestSetup testSetup)
    {
        _serviceProvider = testSetup.ServiceProvider;
        _controller = _serviceProvider.GetService<InfoController>();
    }

    [Fact]
    public void GetFileNamesShouldResultAll()
    {
        //Arrange
        var expected = new List<string> { "big1.dll", "big2.dll", "empty1.txt", "empty2.txt", "long.txt" };

        //Act
        ActionResult<List<string>> result = _controller.GetListOfFiles();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var files = Assert.IsAssignableFrom<List<string>>(okResult.Value);

        Assert.Equal(expected, files);
    }
}
