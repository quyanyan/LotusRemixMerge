using System;
using System.Linq;
using System.Text.RegularExpressions;
using Model;
using Processor.Helpers;
using System.Collections.Generic;
using System.Reflection;

namespace Processor.Parsers
{
    public class WorkExperienceParserCN : IParser
    {
        private static readonly Regex SplitByWhiteSpaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

        private readonly List<string> _jobLookUp;
        private readonly List<string> _countryLookUp;

        public WorkExperienceParserCN(IResourceLoader resourceLoader)
        {
            var assembly = Assembly.GetExecutingAssembly();
            _jobLookUp = new List<string>(resourceLoader.Load(assembly, "JobTitlesCN.txt", ','));
            _countryLookUp = new List<string>(resourceLoader.Load(assembly, "CountriesCN.txt", '|'));
        }

        public void Parse(Section section, Resume resume)
        {
            resume.Positions = new List<Position>();


            bool isFindAValue = false;
            string propertyName = string.Empty;
            Type type;//获取类型
            PropertyInfo propertyInfo;
            string flagValue = string.Empty;

            Position currentPosition = new Position();
            int hasCompanyInfo = 0;
            int flagNotOneTime = 0;//同一经验中不止一个时间节点，标识为1


            for (int i = 0; i < section.Content.Count; i++)
            {
                var line = CommonHelper.CleanInvalidCharsForText(section.Content[i]);

                if (propertyName != string.Empty)
                {
                    type = currentPosition.GetType();//获取类型
                    propertyInfo = type.GetProperty(propertyName);
                    flagValue = propertyInfo.GetValue(currentPosition, null).ToString();
                }

                var startAndEndDate = DateHelper.ParseStartAndEndDateCN(line);    //将时间判断下移 

                string company = "";
                if (GetCompanyNameString(line, out company))//获取公司名字
                {

                    if (flagValue != string.Empty && propertyName == "Company")
                    {
                        resume.Positions.Add(currentPosition);//加集合
                        currentPosition = new Position();
                    }

                    if (!isFindAValue)
                    {
                        propertyName = "Company";
                    }
                    if (line.Substring(line.Length - 1, 1).Equals("："))
                    {
                        currentPosition.Company = section.Content[i + 1];
                    }
                    else
                    {
                        currentPosition.Company = company;
                    }
                    currentPosition.Company = line;
                    isFindAValue = true;
                    continue;
                }
                //判断下一项是不是公司信息，如果是加入集合
                string companyInfo = string.Empty;
                if (GetCompanyInfoString(line, out companyInfo, section))
                {
                    hasCompanyInfo = 1;
                    currentPosition.Summary.Add(companyInfo);
                    i++;
                    continue;
                }
                string title = "";
                if (FindJobTitle(line, out title))//获取Title
                {
                    if (flagValue != string.Empty && propertyName == "Title")
                    {
                        resume.Positions.Add(currentPosition);//加集合
                        currentPosition = new Position();
                    }

                    if (!isFindAValue)
                    {
                        propertyName = "Title";
                    }
                    currentPosition.Title = line;
                    isFindAValue = true;
                    continue;
                }

                string companyDesc = "";
                if (FindJobDescription(line, out companyDesc, section))//获取工作描述
                {
                    if (flagValue != string.Empty && propertyName == "Summary")
                    {
                        resume.Positions.Add(currentPosition);//加集合
                        currentPosition = new Position();
                    }

                    if (!isFindAValue)
                    {
                        propertyName = "Summary";
                    }
                    if (line.Substring(line.Length - 1, 1).Equals("："))
                    {
                        int a = 0;
                        for (int j = i; j < section.Content.Count - 1; j++)
                        {
                            if (DateHelper.ParseStartAndEndDateCN(section.Content[j]) == null && string.IsNullOrEmpty(FindJobTitle(section.Content[j])))
                            {
                                currentPosition.Summary.Add(CommonHelper.CleanInvalidCharsForText(section.Content[j + 1]));
                            }
                            a = j;
                        }
                        i = a;
                    }
                    else
                    {
                        currentPosition.Summary.Add(line);
                    }
                    isFindAValue = true;
                }

                if (startAndEndDate != null)
                {
                    if (hasCompanyInfo == 1)
                    {
                        flagNotOneTime++;
                    }
                    if (flagValue != string.Empty && propertyName == "StartDate")
                    {
                        if (flagNotOneTime == 0|| flagNotOneTime % 2 == 0)
                        {
                            resume.Positions.Add(currentPosition);//加集合
                            currentPosition = new Position();
                            hasCompanyInfo = 0;
                            flagNotOneTime = 0;
                        }
                    }
                    else
                    {
                        isFindAValue = false;
                    }

                    if (!isFindAValue)
                    {
                        propertyName = "StartDate";
                    }

                    if (flagNotOneTime == 0|| flagNotOneTime % 2 == 0)
                    {
                        currentPosition.StartDate = startAndEndDate.Start;
                        currentPosition.EndDate = startAndEndDate.End;
                    }
                    isFindAValue = true;
                    continue;
                }

                if (i == (section.Content.Count - 1))
                {
                    resume.Positions.Add(currentPosition);//加集合
                }
            }
        }
        private bool GetCompanyNameString(string line, out string companyName)
        {

            List<string> list = new List<string>() { "集团", "有限公司", "责任公司", "技术公司", "科技公司", "猎聘网" };
            companyName = "";
            bool result = false;
            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                companyName = line;
                result = true;
            }
            return result;
        }
        private bool GetCompanyInfoString(string line, out string companyInfo, Section section)
        {
            List<string> list = new List<string>() { "公司描述", "公司性质", "公司规模", "公司行业" };
            companyInfo = "";
            bool result = false;
            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                if (line.Substring(line.Length - 1, 1).Equals("："))
                {
                    int a = section.Content.IndexOf(line);
                    companyInfo = line + CommonHelper.CleanInvalidCharsForText(section.Content[a + 1]);
                }
                else
                {
                    companyInfo = line;
                }
                result = true;
            }
            return result;
        }
        private bool FindJobDescription(string line, out string companyDesc, Section section)
        {
            List<string> list = new List<string>() { "工作描述", "责任描述", "岗位职责" };
            companyDesc = "";
            bool result = false;
            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                if (line.Substring(line.Length - 1, 1).Equals("："))
                {
                    int a = section.Content.IndexOf(line);
                    companyDesc = line + CommonHelper.CleanInvalidCharsForText(section.Content[a + 1]);
                }
                else
                {
                    companyDesc = line;
                }
                result = true;
            }
            return result;
        }

        private bool FindJobTitle(string line, out string title)
        {
            title = "";
            bool result = false;
            if (_jobLookUp.Any(x => line.IndexOf(x) >= 0))
            {
                title = line;
                result = true;
            }
            return result;
        }
        private string FindJobTitle(string line)
        {
            var elements = SplitByWhiteSpaceRegex.Split(line);
            if (elements.Length > 4)
            {
                return string.Empty;
            }

            return _jobLookUp.FirstOrDefault(job => line.IndexOf(job, StringComparison.InvariantCultureIgnoreCase) > -1);
        }
    }
}
