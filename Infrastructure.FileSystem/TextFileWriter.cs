using System.Text;
using Domain;

namespace Infrastructure.FileSystem;

public class TextFileWriter : ITextWriter
{
    public TextFileWriter(FileInfo destination)
    {
        Destination = destination;
    }

    private FileInfo Destination { get; }


    public void Write(IEnumerable<string> lines)
    {
        File.WriteAllLines(Destination.FullName, lines, Encoding.UTF8);
    }
}