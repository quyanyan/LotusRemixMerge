using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Processor.Helpers;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Processor.Parsers
{
    public class EducationParserCN : IParser
    {
        private static readonly List<string> _schoolLookUp =new List<string>
        {"学校","学院","研究所", "机构","大学","学院", "中学"};
        private static readonly List<string> _courseLookUp = new List<string>
        {"学士","硕士", "博士","本科", "专科","大专", "中专","高中", "小学" };

        private static readonly Regex titileRegex = new Regex(@"\(\) ");

        public void Parse(Section section, Resume resume)
        {
            resume.Educations = new List<Education>();
            bool isFindAValue = false;
            string propertyName = string.Empty;
            System.Type type;//获取类型
            PropertyInfo propertyInfo;
            string flagValue = string.Empty;

            Education currentEducation = null;
            for (int i = 0; i < section.Content.Count; i++)
            {
                //var line = DateHelper.CleanInvalidCharsForText(section.Content[i]);
                var line =section.Content[i];
                if (currentEducation == null)
                {
                    currentEducation = new Education();
                }
                if (propertyName != string.Empty)
                {
                    type = currentEducation.GetType();//获取类型
                    propertyInfo = type.GetProperty(propertyName);
                    flagValue = propertyInfo.GetValue(currentEducation, null).ToString();
                }
                var startAndEndDate = DateHelper.ParseStartAndEndDateCN(line);//毕业时间
                if (startAndEndDate != null)
                {
                    if (flagValue != string.Empty && propertyName == "StartDate")
                    {
                        resume.Educations.Add(currentEducation);//加集合
                        currentEducation = new Education();
                    }

                    if (!isFindAValue)
                    {
                        propertyName = "StartDate";
                    }
                    currentEducation.StartDate = startAndEndDate.Start;
                    currentEducation.EndDate = startAndEndDate.End;
                    isFindAValue = true;
                    continue;
                }
                var school = ParseSchool(line);//毕业学校
                if (!string.IsNullOrEmpty(school) )//|| !string.IsNullOrEmpty(course))
                {
                    if (flagValue != string.Empty && propertyName == "School")
                    {
                        resume.Educations.Add(currentEducation);//加集合
                        currentEducation = new Education();
                    }
                    if (!isFindAValue)
                    {
                        propertyName = "School";
                    }
                    var listStr = line.Split(' ');
                   
                    if (listStr.Count()>1)
                    {
                        currentEducation.School = listStr.First();
                        string str = "";
                        for (int j = 0; j < listStr.Count(); j++)
                        {
                            if (j!=0)
                            {
                                str += listStr[j];
                            }
                        }
                        currentEducation.Course = str;
                    }
                    else
                    {
                        currentEducation.School = school;
                    }
                    isFindAValue = true;
                }
                var course = _courseLookUp.FirstOrDefault(
                                                 c => line.IndexOf(c, StringComparison.InvariantCultureIgnoreCase) > -1);

                if (!string.IsNullOrEmpty(course))
                {

                    if (flagValue != string.Empty && propertyName == "Course")
                    {
                        resume.Educations.Add(currentEducation);//加集合
                        currentEducation = new Education();
                    }

                    if (!isFindAValue)
                    {
                        propertyName = "Course";
                    }
                    var listStr = line.Split(' ');

                    if (listStr.Count()>1)
                    {
                        currentEducation.School = listStr.First();
                        string str = "";
                        for (int j = 0; j < listStr.Count(); j++)
                        {
                            if (j != 0)
                            {
                                str += listStr[j];
                            }
                        }
                        currentEducation.Course = str;
                    }
                    else
                    {
                        currentEducation.Course = line;
                    }
                    isFindAValue = true;
                }

                if (i == (section.Content.Count - 1))
                {
                    resume.Educations.Add(currentEducation);//加集合
                }
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
            var startAndEndDate = DateHelper.ParseStartAndEndDateCN(line);
            if (startAndEndDate != null)
            {
                currentEducation.StartDate = startAndEndDate.Start;
                currentEducation.EndDate = startAndEndDate.End;
            }
        }
    }
}