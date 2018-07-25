using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Models;
using Processor.Helpers;

namespace Processor.Parsers
{
    public class ResumeBuilder
    {
        private readonly Dictionary<SectionType, dynamic> _parserRegistry;
        public ResumeBuilder(IResourceLoader resourceLoader, bool isChinese)
        {
            //TODO:解析
            if (isChinese)
            {
                _parserRegistry = new Dictionary<SectionType, dynamic>
            {
                {SectionType.Personal, new PersonalParserCN(resourceLoader)},
                {SectionType.Summary, new SummaryParser()},
                {SectionType.Education, new EducationParserCN()},
                {SectionType.Projects, new ProjectsParserCN()},
                {SectionType.WorkExperience, new WorkExperienceParserCN(resourceLoader)},
                {SectionType.Skills, new SkillsParserCN()},
                {SectionType.Courses, new CoursesParserCN()},
                {SectionType.Awards, new AwardsParserCN()}
            };
            }
            else
            {
                _parserRegistry = new Dictionary<SectionType, dynamic>
                {
                    {SectionType.Personal, new PersonalParser(resourceLoader)},
                    {SectionType.Summary, new SummaryParser()},
                    {SectionType.Education, new EducationParser()},
                    {SectionType.Projects, new ProjectsParser()},
                    {SectionType.WorkExperience, new WorkExperienceParser(resourceLoader)},
                    {SectionType.Skills, new SkillsParser()},
                    {SectionType.Courses, new CoursesParser()},
                    {SectionType.Awards, new AwardsParser()}
                };
            }

        }

        public Resume Build(IList<Section> sections)
        {
            var resume = new Resume();

            foreach (var section in sections.Where(section => _parserRegistry.ContainsKey(section.Type)))
            {
                _parserRegistry[section.Type].Parse(section, resume);
            }

            return resume;
        }
    }
}
