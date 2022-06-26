namespace Domain.TextProcessing;

class ChainedProcessor : ITextProcessor
{
    private ITextProcessor Inner { get; }
    private ITextProcessor Next { get; }

    public ChainedProcessor(ITextProcessor inner, ITextProcessor next)
    {
        Inner = inner;
        Next = next;
    }

    public IEnumerable<string> Execute(IEnumerable<string> text) =>
        Next.Execute(Inner.Execute(text));
}