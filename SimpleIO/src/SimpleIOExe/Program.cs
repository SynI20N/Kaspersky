using System.IO;
using SimpleIOExe.Interfaces;
using SimpleIOExe.Services;

string path = Path.Combine(".", "students.txt");
Random rnd = new Random();
FileLineWriter flw = new FileLineWriter(path, false);
RandomLineGenerator rlg = new RandomLineGenerator(rnd);

using (StudentFileCreator sfc = new StudentFileCreator(flw, rlg))
{
    sfc.Create(10);
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
