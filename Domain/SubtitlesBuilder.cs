using Domain.Models;
using Domain.TextProcessing;

namespace Domain;

public class SubtitlesBuilder
{
    private ITextReader Reader { get; set; } = TextReader.Empty;
    private ITextProcessor Processing { get; set; } = new DoNothing();

    public SubtitlesBuilder For(ITextReader source)
    {
        Reader = source;
        return this;
    }
    
    public SubtitlesBuilder Using(ITextProcessor processor)
    {
        Processing = Processing.Then(processor);
        return this;
    }
    
    public Subtitles Build()
    {
        TimedText processed = Reader.Read().Apply(Processing);

        TextDurationMeter durationMeter = new TextDurationMeter(processed);
        IEnumerable<SubtitleLine> subtitles = durationMeter.MeasureLines();
        return new Subtitles(subtitles);
    }
}