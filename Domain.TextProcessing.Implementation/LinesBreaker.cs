using System.Text.RegularExpressions;
using Common;

namespace Domain.TextProcessing.Implementation
{
    public class LinesBreaker : ITextProcessor
    {
        public int MaxLineLength { get; }
        public int MinLengthToBreakInto { get; }

        public LinesBreaker(int maxLineLength, int minLengthToBreakInto)
        {
            MaxLineLength = maxLineLength;
            MinLengthToBreakInto = minLengthToBreakInto;
        }
        
        private IEnumerable<(string separatorPattern, string appendLeft, string prependRight)[]> 
            Rules { get; } = new []
        {
            new[] 
            {            
                (", ", "...", "... "),
                ("; ", "...", "... "),
                (" - ", "...", "... "),
            },
            new[] 
            {            
                (" and ", "...", "... and "),
                (" or ", "...", "... or "),
            },
            new[] 
            {            
                (" to ", "...", "... to "),
                (" then ", "...", "... then "),
            },
            new[] 
            {            
                (" ", "...", "... ")
            },
        };

        public IEnumerable<string> Execute(IEnumerable<string> text) =>
            text.SelectMany(BreakLongLine);

        public IEnumerable<string> BreakLongLine(string line)
        {
            string remaining = line;

            while (remaining.Length > 0)
            {
                if (remaining.Length <= MaxLineLength)
                {
                    yield return remaining;
                    break;
                }

                bool broken = false;
                foreach ((string separator, string toLeft, string toRight)[] rules in Rules)
                {
                    IEnumerable<(string left, string right)> split =
                        TryBreakLongLine(remaining, rules)
                            .ToList();

                    if (split.Any())
                    {
                        (string left, string right) = split.First();
                        yield return left;
                        remaining = right;
                        broken = true;
                        break;
                    }
                }

                if (!broken)
                {
                    yield return remaining;
                    break;
                }
            }
        }

        private IEnumerable<(string left, string right)> TryBreakLongLine(
            string line, 
            IEnumerable<(string separatorPattern, string appendLeft, string prependRight)> rules) =>
            rules.SelectMany(rule => BreakLongLine(line, rule))
                .WithMinimumOrEmpty(split => MaxLineLength - split.left.Length);

        private IEnumerable<(string left, string right)> BreakLongLine(
            string line, 
            (string separatorPattern, string appendLeft, string prependRight) rule) => 
            new Regex(rule.separatorPattern).Matches(line)
                .Select(match => (
                    left: line.Substring(0, match.Index) + rule.appendLeft, 
                    right: rule.prependRight + line.Substring(match.Index + match.Length)))
                .Where(split => 
                    MinLengthToBreakInto <= split.left.Length && 
                    split.left.Length <= MaxLineLength);
    }
}
