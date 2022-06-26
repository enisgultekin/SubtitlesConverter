namespace Domain;

public interface ITextWriter
{
    void Write(IEnumerable<string> lines);
}