using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processor.Parsers
{
    public class CoursesParserCN : IParser
    {
        public void Parse(Section section, Resume resume)
        {
            resume.Courses = section.Content;
        }
    }
}
