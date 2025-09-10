namespace Kasp.Test.Interfaces
{
    public interface ITestFileGeneratorService
    {
        void GenerateTestFiles(string baseDir, int subDirsCount, int filesPerSubDir);
    }
}
