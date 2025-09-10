using Xunit;
using SimpleIOExe.Interfaces;
using SimpleIOExe.Services;
using System.IO;

namespace SimpleIOExe.Tests;

public class TestDifferentLengths
{
    private static void RunWholeProgram(int fileLength, string path)
    {
        Random rnd = new Random();
        FileLineWriter flw = new FileLineWriter(path, false);
        RandomLineGenerator rlg = new RandomLineGenerator(rnd);

        using (StudentFileCreator sfc = new StudentFileCreator(flw, rlg))
        {
            sfc.Create(fileLength);
        }

        IReader reader;
        try
        {
            reader = new FileLineReader(path);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
        IWriter writer = new ConsoleStreamer();

        using (CommonStudentFileStreamer csfs = new CommonStudentFileStreamer(reader, writer))
        {
            csfs.StreamFile();
        }
        using (FileLineWriter flw2 = new FileLineWriter(path, true))
        {
            flw2.WriteLine(rlg.GetRandomLine());
        }
    }

    [Theory]
    [InlineData(10)]
    [InlineData(1000000)]
    [InlineData(20000000)]
    public void TestShortFile(int length)
    {
        //Arrange
        string path = Path.Combine(".", "students.txt");

        //Act
        RunWholeProgram(length, path);

        //Assert
        long actualLength = new FileInfo(path).Length;
        Assert.True(actualLength > length * 18);
    }
}
