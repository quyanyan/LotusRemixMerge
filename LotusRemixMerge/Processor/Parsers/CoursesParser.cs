using Model;
using Model.Models;

namespace Processor.Parsers
{
    public class CoursesParser : IParser
    {        
        public void Parse(Section section, Resume resume)
        {
            resume.Courses = section.Content;
        }
    }
}
