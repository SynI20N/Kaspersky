namespace ZipperAPI.Services;

public interface IFolderService
{
    public string GetRelativeFilesPath();
    public string GetRelativeArchivePath();
    public string GetWorkingDir();
    public string GetArchiverPath();
    public string GetArchivesPath();
    public string GetOutputPath(int processId);
}
