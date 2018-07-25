using System.Collections.Generic;
using System.Linq;
using Model.Models;
using System.Text.RegularExpressions;

namespace Processor
{
    public class SectionMatchingService
    {
        //TODO:确定切割词
        private readonly Dictionary<SectionType, List<string>> _keyWordRegistry = new Dictionary<SectionType, List<string>>
        {                        
            {SectionType.Education, new List<string> {"教育经历", "education background", "education", "education", "study", "school", "degree", "institution", "academic", "qualification" } },
            {SectionType.Courses, new List<string> {"coursework", "course"}},
            {SectionType.Summary, new List<string> {"summary","profile","自我描述","自我评价"}},
            {SectionType.WorkExperience, new List<string> {"工作经验", "工作经历","experience", "work", "employment"}},
            {SectionType.Projects, new List<string> {"项目经验", "项目经历", "project experience","projects"}},
            {SectionType.Skills, new List<string> {"技能特长","专业技能","professional skills","skills","skill", "ability", "tool","技能评价"}},
            {SectionType.Awards, new List<string> {"附加信息","证书","award", "certification", "certificate"}}
        };

        private static readonly Regex workAgeRegex = new Regex("\\S\\d+年工作经验");
        private static readonly Regex workAgesRegex = new Regex("(\\d+年工作经验)|(\\S工作经验)");
        private static readonly Regex eduRegex = new Regex("\\S教育经历");
        private static readonly Regex workRegex = new Regex("\\S工作经历");
        private static readonly Regex awardRegex = new Regex("(\\S自我评价)");
        private static readonly Regex skilldRegex = new Regex("(\\S专业技能)");

        //public SectionType FindSectionTypeMatching(string input)
        //{
        //    return
        //        (from sectionType in _keyWordRegistry
        //         where sectionType.Value.Any(input.Contains)
        //         select sectionType.Key).FirstOrDefault();
        //}

        public SectionType FindSectionTypeMatching(string input)
        {
            if(workAgeRegex.Match(input).Success)
            {
                return SectionType.Unknown;
            }
            if (workAgesRegex.Match(input).Success)
            {
                return SectionType.Unknown;
            }
            if (eduRegex.Match(input).Success)
            {
                return SectionType.Unknown;
            }
            if (workRegex.Match(input).Success)
            {
                return SectionType.Unknown;
            }
            if (awardRegex.Match(input).Success)
            {
                return SectionType.Unknown;
            }
            if (skilldRegex.Match(input).Success)
            {
                return SectionType.Unknown;
            }

            return
                (from sectionType in _keyWordRegistry
                 where sectionType.Value.Any(input.Contains)
                 select sectionType.Key).FirstOrDefault();
        }

    }
}
