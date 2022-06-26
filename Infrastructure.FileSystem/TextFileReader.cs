using System.Text;
using System.Text.RegularExpressions;
using Domain;
using Domain.Models;

namespace Infrastructure.FileSystem;

public class TextFileReader : ITextReader
{
    public TimedText Read() => ParseSource();
    private FileInfo Source { get; }
    
    public TextFileReader(FileInfo source)
    {
        Source = source;
    }
    
    private TimedText ParseSource()
    {
        if(Source is null) return TimedText.Empty;

        TimeSpan? initial = null;
        TimeSpan? final = null;
        List<string> content = new List<string>();
        bool beginsInTimeSpan = true;
        bool endedInTimeSpan = false;
        foreach (var line in File.ReadLines(Source.FullName,Encoding.UTF8))
        {
            if (Parse(line) is TimeSpan time)
            {
                initial = initial ?? time;
                final = time;
                endedInTimeSpan = true;
            }
            else
            {
                content.Add(line);
                beginsInTimeSpan = beginsInTimeSpan && initial.HasValue;
                endedInTimeSpan = false;
            }
        }

        if (!beginsInTimeSpan || !endedInTimeSpan)
            throw new InvalidOperationException(
                "Source file is not structured correctly.");

        TimeSpan duration = final.Value.Subtract(initial.Value);
        return new TimedText(content, duration);
    }

    private object Parse(string line)
    {
        Regex timePattern = new Regex(@"^\s*(?<minutes>\d+):(?<seconds>\d+)\s*$");
        Match match = timePattern.Match(line);

        if (!match.Success) return line;

        int minutes = int.Parse(match.Groups["minutes"].Value);
        int seconds = int.Parse(match.Groups["seconds"].Value);

        return new TimeSpan(0, minutes, seconds);

    }
}