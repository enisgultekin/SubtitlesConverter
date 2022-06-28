namespace Domain.TextProcessing.Implementation;

class TwoWayRepeater : IMultiwaySplitter
{
    public TwoWayRepeater(ITwoWaySplitter splitter)
    {
        Splitter = splitter;
    }

    private ITwoWaySplitter Splitter { get; }
    
    public IEnumerable<string> ApplyTo(string line)
    {
        string remaining = line.Trim();
        while (remaining.Length > 0)
        {
            (string extracted, string rest) = Splitter.ApplyTo(remaining).First();

            yield return extracted;
            remaining = rest;
        }
    }
}