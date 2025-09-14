using System.Diagnostics;

namespace ZipperAPI.Services;

public class FolderService : IFolderService
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;

    public FolderService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        _env = webHostEnvironment;
        _configuration = configuration;
    }

    public string GetRelativeFilesPath()
    {
        var path = _configuration["FolderPath"]
            ?? throw new Exception("FolderPath is not configured");
        return path;
    }

    public string GetRelativeArchivePath()
    {
        var path = _configuration["ArchivePath"]
            ?? throw new Exception("ArchivePath is not configured");
        return path;
    }

    public string GetWorkingDir()
    {
        var path = GetRelativeFilesPath();
        return Path.Combine(_env.ContentRootPath, path);
    }

    public string GetArchiverPath()
    {
        var archiverPath = Environment.GetEnvironmentVariable("ARCHIVER_PATH")
            ?? throw new InvalidOperationException("ArchiverPath not configured");
        return archiverPath;
    }

    public string GetOutputPath(int processId)
    {
        var archivePath = GetRelativeArchivePath();
        var outputPath = Path.Combine(_env.ContentRootPath, archivePath, $"archive_{processId}.zip");
        return outputPath;
    }

    public string GetArchivesPath()
    {
        var archivePath = GetRelativeArchivePath();
        var outputPath = Path.Combine(_env.ContentRootPath, archivePath);
        return outputPath;
    }
}
