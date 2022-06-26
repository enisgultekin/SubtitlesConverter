using System.Collections.Generic;
using System.Linq;

namespace SubtitlesConverter.Domain.TextProcessing;

class Pipeline : ITextProcessor
{
    public IEnumerable<ITextProcessor> Processors { get; }

    public Pipeline(IEnumerable<ITextProcessor> processors)
    {
        Processors = processors;
    }
    
    public Pipeline(params ITextProcessor[] processors)
    :this((IEnumerable<ITextProcessor>)processors)
    {
    }

    public IEnumerable<string> Execute(IEnumerable<string> text) =>
        Processors.Aggregate(text, (current, processor) => processor.Execute(current));
}