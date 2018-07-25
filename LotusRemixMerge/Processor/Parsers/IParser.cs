using Model;
using Model.Models;

namespace Processor.Parsers
{
    public interface IParser
    {
        void Parse(Section section, Resume resume);
    }
}
