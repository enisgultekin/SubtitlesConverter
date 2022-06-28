namespace Domain.TextProcessing.Implementation;

public interface IMultiwaySplitter
{
    IEnumerable<string> ApplyTo(string line);
}