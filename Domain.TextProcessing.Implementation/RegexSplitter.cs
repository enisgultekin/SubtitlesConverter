using System.Text.RegularExpressions;

namespace Domain.TextProcessing.Implementation;

internal class RegexSplitter : ITwoWaySplitter
{
    private Regex Pattern { get; }
    public Func<Match, string> ExtractLeft { get; }
    public Func<Match, string> ExtractRight { get; }

    public RegexSplitter(Regex pattern, Func<Match, string> extractLeft, Func<Match, string> extractRight)
    {
        Pattern = pattern;
        ExtractLeft = extractLeft;
        ExtractRight = extractRight;
    }

    public IEnumerable<(string left, string right)> ApplyTo(string line)
    {
        return Pattern
            .Matches(line)
            .Select(match => (ExtractLeft(match), ExtractRight(match)));
    }

    public static ITwoWaySplitter LeftAndRightExtractor(string pattern) =>
        new RegexSplitter(new Regex(pattern), match => match.Groups["left"].Value,
            match => match.Groups["right"].Value);

    public static ITwoWaySplitter LeftExtractor(string pattern) =>
        new RegexSplitter(new Regex(pattern), match => match.Groups["left"].Value,
            _ => string.Empty);
}