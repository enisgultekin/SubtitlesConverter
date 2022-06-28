namespace Domain.TextProcessing.Implementation;

interface ITwoWaySplitter
{
    IEnumerable<(string left, string right)> ApplyTo(string line);
}