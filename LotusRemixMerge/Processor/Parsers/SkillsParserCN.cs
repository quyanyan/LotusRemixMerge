using System.Collections.Generic;
using System.Linq;
using Model;

namespace Processor.Parsers
{
    public class SkillsParserCN : IParser
    {
        public void Parse(Section section, Resume resume)
        {
            resume.Skills = new List<string>();

            foreach (var line in section.Content)
            {
                var indexOfColon = line.IndexOf(':');
                var skills = indexOfColon > -1 ? line.Substring(indexOfColon + 1) : line;
                var elements = skills.Split(',');
                resume.Skills.AddRange(elements.Select(e => e.Trim()));
            }
        }
    }
}
