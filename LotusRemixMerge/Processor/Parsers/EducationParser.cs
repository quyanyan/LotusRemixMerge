using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Models;
using Processor.Helpers;

namespace Processor.Parsers
{
    public class EducationParser : IParser
    {
        private static readonly List<string> _schoolLookUp =
            new List<string>
            {
                "学校",
                "学院",
                "研究所",
                "机构",
                "大学",
                "学院",
                "中学"
            };
        private static readonly List<string> _courseLookUp = new List<string>
        {
            "学士",
            "硕士",
            "博士",
            "本科",
            "专科",
            "大专",
            "中专",
            "高中",
            "小学"
        };

        public void Parse(Section section, Resume resume)
        {
            resume.Educations = new List<Education>();

            var i = 0;
            Education currentEducation = null;
            while (i < section.Content.Count)
            {
                var line = section.Content[i];
                var school = ParseSchool(line);
                if (string.IsNullOrWhiteSpace(school))
                {
                    if (currentEducation != null)
                    {
                        ParseStartAndEndDate(line, currentEducation);

                        var course =
                            _courseLookUp.FirstOrDefault(
                                c => line.IndexOf(c, StringComparison.InvariantCultureIgnoreCase) > -1);
                        if (!string.IsNullOrWhiteSpace(course))
                        {
                            currentEducation.Course = line;
                        }
                    }
                }
                else
                {
                    if (currentEducation != null && string.IsNullOrWhiteSpace(currentEducation.Course))
                    {
                        currentEducation.School += ", " + school;
                    }
                    else
                    {
                        currentEducation = new Education
                        {
                            School = school
                        };

                        ParseStartAndEndDate(line, currentEducation);

                        resume.Educations.Add(currentEducation);
                    }                                                            
                }

                i++;
            }
        }

        private static string ParseSchool(string line)
        {
            var school = _schoolLookUp.FirstOrDefault(s => line.IndexOf(s, StringComparison.InvariantCultureIgnoreCase) > -1);
            if (string.IsNullOrWhiteSpace(school))
            {
                return string.Empty;
            }

            var endOfSchoolIndex = line.IndexOf('\t');
            if (endOfSchoolIndex == -1)
            {
                endOfSchoolIndex = line.Length - 1;
            }

            return line.Substring(0, endOfSchoolIndex + 1);
        }

        private static void ParseStartAndEndDate(string line, Education currentEducation)
        {
            var startAndEndDate = DateHelper.ParseStartAndEndDate(line);
            if (startAndEndDate != null)
            {
                currentEducation.StartDate = startAndEndDate.Start;
                currentEducation.EndDate = startAndEndDate.End;
            }
        }
    }
}
