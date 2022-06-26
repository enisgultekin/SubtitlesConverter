using System.Text.RegularExpressions;
using Domain.Models;

namespace Domain
{
    internal class TextDurationMeter
    {
        public TimedText Text { get; }

        internal TextDurationMeter(TimedText text)
        {
            Text = text;
        }

        public IEnumerable<SubtitleLine> MeasureLines() =>
            MeasureLines(GetFullTextWeight());

        private IEnumerable<SubtitleLine> MeasureLines(double fullTextWeigh) =>
            Text.Content
                .Select(line => (
                    line: line,
                    relativeWeight: CountReadableLetters(line) / fullTextWeigh))
                .Select(tuple => (
                    line: tuple.line,
                    miliseconds: Text.Duration.TotalMilliseconds * tuple.relativeWeight))
                .Select(tuple => new SubtitleLine(tuple.line, TimeSpan.FromMilliseconds(tuple.miliseconds)));

        private double GetFullTextWeight() =>
            Text.Content.Sum(CountReadableLetters);

        private int CountReadableLetters(string text) =>
            Regex.Matches(text, @"\w+")
                .Sum(match => match.Value.Length);
    }
}