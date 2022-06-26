namespace Domain.TextProcessing
{
    class DoNothing : ITextProcessor
    {
        public IEnumerable<string> Execute(IEnumerable<string> text) => text;
    }
}