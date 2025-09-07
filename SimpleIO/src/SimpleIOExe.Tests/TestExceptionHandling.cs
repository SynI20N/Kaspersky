using SimpleIOExe.Interfaces;
using SimpleIOExe.Services;
using System.IO;

namespace SimpleIOExe.Tests;

public class TestExceptionHandling
{
    [Fact]
    public void TestFileDeletedBeforeReading()
    {
        //Arrange
        string path = Path.Combine(".", "students.txt");
        Random rnd = new Random();
        FileLineWriter flw = new FileLineWriter(path, false);
        RandomLineGenerator rlg = new RandomLineGenerator(rnd);

        using (StudentFileCreator sfc = new StudentFileCreator(flw, rlg))
        {
            sfc.Create(10);
        }

        //Act
        File.Delete(path);

        //Assert
        Assert.Throws<FileNotFoundException>(() =>
        {
            IReader reader = new FileLineReader(path);
        });
    }

    [Fact]
    public void TestFileWrongFormat()
    {
        //Arrange
        string path = Path.Combine(".", "students.txt");
        Random rnd = new Random();
        RandomLineGenerator rlg = new RandomLineGenerator(rnd);
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.WriteLine("This is my fabulous format for checking what happens");
        }

        //Act
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

        //Assert
        Assert.Throws<FormatException>(() =>
        {
            using (CommonStudentFileStreamer csfs = new CommonStudentFileStreamer(reader, writer))
            {
                csfs.StreamFile();
            }
            using (FileLineWriter flw2 = new FileLineWriter(path, true))
            {
                flw2.WriteLine(rlg.GetRandomLine());
            }
        });
    }
}
