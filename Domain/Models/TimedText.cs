using Domain.TextProcessing;

namespace Domain.Models;

public class TimedText
{
    public IEnumerable<string> Content { get; }
    public TimeSpan Duration { get; }
    public static TimedText Empty { get; } = new(Enumerable.Empty<string>(), TimeSpan.Zero);

    public TimedText(IEnumerable<string> content, TimeSpan duration)
    {
        Content = content;
        Duration = duration;
    }

    public TimedText Apply(ITextProcessor processor) => new(processor.Execute(Content), Duration);
}