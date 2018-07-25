using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processor.Parsers
{
    public class AwardsParserCN : IParser
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
