using Common;

namespace Domain.TextProcessing.Implementation;

internal class ShortextLeftWins : ITwoWaySplitter
{
    private IEnumerable<ITwoWaySplitter> Splitters { get; }
        
    public ShortextLeftWins(IEnumerable<ITwoWaySplitter> splitters)
    {
        Splitters = splitters;
    }
        
    public IEnumerable<(string left, string right)> ApplyTo(string line)
    {
        return Splitters
            .SelectMany(rule => rule.ApplyTo(line))
            .DefaultIfEmpty((left: line, right: string.Empty))
            .WithMinimumOrEmpty(tuple => tuple.left.Length);
    }
}