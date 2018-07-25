using Model;
using System.Collections.Generic;

namespace Processor.Parsers
{
    public class AwardsParser : IParser
    {
        public void Parse(Section section, Resume resume)
        {
            //resume.Awards = section.Content;
            resume.Awards = new List<string>();

            for (int i = 0; i < section.Content.Count; i++)
            {
                resume.Awards.Add(Helpers.CommonHelper.CleanInvalidCharsForText(section.Content[i]));
            }
        }
    }
}
