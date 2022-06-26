namespace Domain.TextProcessing.Implementation
{
    public class LinesTrimmer : ITextProcessor
    {
        public IEnumerable<string> Execute(IEnumerable<string> text) =>
            text
                .Select(line => line.Trim())
                .Where(line => line.Length > 0);
    }
}