using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatisticsAPI.Controllers;

namespace StatisticsAPI.Tests;

public class FileControllerTests
{
    private readonly FileController _controller;

    public FileControllerTests(FileController controller)
    {
        _controller = controller;
    }

    [Fact]
    public async Task TestUploadWrongFormat()
    {
        //Arrange
        var content = "Hello world hehe";
        var fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;
        //Create IFormFile
        IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
        CancellationToken tok = new CancellationToken();

        //Act
        var result = await _controller.UploadAsync(file, tok);
        BadRequestObjectResult? badRequest = result as BadRequestObjectResult;

        //Assert
        Assert.NotNull(badRequest);
        Assert.Equal("wrong file format", badRequest.Value);
    }

    [Fact]
    public async Task TestUploadEmpty()
    {
        //Arrange
        var content = "";
        var fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;
        //Create IFormFile
        IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
        CancellationToken tok = new CancellationToken();

        //Act
        var result = await _controller.UploadAsync(file, tok);
        BadRequestObjectResult? badRequest = result as BadRequestObjectResult;

        //Assert
        Assert.NotNull(badRequest);
        Assert.Equal("file empty", badRequest.Value);
    }


}
