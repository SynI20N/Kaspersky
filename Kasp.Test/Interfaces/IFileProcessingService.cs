namespace Kasp.Test.Interfaces
{
    public interface IFileProcessingService
    {
        void ProcessFile(string filePath, string extension);
        void ProcessDirectory(string path);
    }

}
