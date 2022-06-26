using Domain.Models;

namespace Domain;

public interface ITextReader
{
    TimedText Read();
}