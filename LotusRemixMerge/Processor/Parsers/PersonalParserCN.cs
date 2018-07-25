using Model;
using Model.Models;
using Processor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Processor.Parsers
{
    public class PersonalParserCN : IParser
    {
        private static readonly Regex EmailRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex PhoneRegex1 = new Regex(@"^(13|15|18|17)[0-9]{9}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex PhoneRegex = new Regex(@"1[3578]((\d{1}(\*\*\*\*)\d{4})|(\d{9}))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SocialProfileRegex = new Regex(@"(http(s)?:\/\/)?([\w]+\.)?(linkedin\.com|facebook\.com|github\.com|stackoverflow\.com|bitbucket\.org|sourceforge\.net|(\w+\.)?codeplex\.com|code\.google\.com).*?(?=\s)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SplitByWhiteSpaceRegex = new Regex(@"\s+|,", RegexOptions.Compiled);
        private static readonly Regex AgeRegex = new Regex(@"(^\d+岁)|(^[0-9]\d$)");

        private readonly HashSet<string> _firstNameLookUp;
        private readonly List<string> _countryLookUp;


        public PersonalParserCN(IResourceLoader resourceLoader)
        {
            var assembly = Assembly.GetExecutingAssembly();

            _firstNameLookUp = resourceLoader.LoadIntoHashSet(assembly, "FirstNameCN.txt", ',');
            _countryLookUp = new List<string>(resourceLoader.Load(assembly, "CountriesCN.txt", '|'));
        }

        public void Parse(Section section, Resume resume)
        {
            var firstNameFound = false;
            var addressFound = false;
            var genderFound = false;
            var ageFound = false;
            var emailFound = false;
            var phoneFound = false;
            var commentFound = false;
            var GraduateSchool = false;
            var Major = false;
            var Edu = false;
            var MaritalStatus = false;
            var Profession = false;
            var DutyTime = false;
            var JobNature = false;
            var JobStatus = false;
            var Curposition = false;
            var ExpectedSalary = false;
            var TargetWorkPlace = false;

            foreach (var lineStr in section.Content)
            {
                string line = lineStr.Replace("/", "").Replace("\v", "|");
                if (!string.IsNullOrEmpty(CommonHelper.CleanInvalidCharsForText(line)))
                {
                    firstNameFound = ExtractFirstAndLastName(resume, firstNameFound, line);

                    genderFound = ExtractGender(resume, genderFound, line);

                    ageFound = ExtractAge(resume, ageFound, line);

                    emailFound = ExtractEmail(resume, emailFound, line);

                    phoneFound = ExtractPhone(resume, phoneFound, line);

                    //addressFound = ExtractAddress(resume, addressFound, line);

                    addressFound = ExtractCurrentAddress(resume, addressFound, line);

                    GraduateSchool = ExtractGraduateSchool(resume, GraduateSchool, line, section);
                    Major = ExtractMajor(resume, Major, line, section);
                    Edu = ExtractEdu(resume, Edu, line, section);
                    MaritalStatus = ExtractMaritalStatus(resume, MaritalStatus, line, section);
                    Profession = ExtractProfession(resume, Profession, line, section);
                    DutyTime = ExtractDutyTime(resume, DutyTime, line, section);
                    JobNature = ExtractJobNature(resume, JobNature, line, section);
                    JobStatus = ExtractJobStatus(resume, JobStatus, line, section);
                    Curposition = ExtractCurposition(resume, Curposition, line, section);
                    ExpectedSalary = ExtractExpectedSalary(resume, ExpectedSalary, line, section);
                    TargetWorkPlace = ExtractTargetWorkPlace(resume, TargetWorkPlace, line, section);
                    commentFound = ExtractComment(resume, commentFound, line, section);
                    ExtractSocialProfiles(resume, line);
                }
            }
        }

        private bool ExtractAddress(Resume resume, bool addressFound, string line)
        {
            if (addressFound) return addressFound;

            var country =
                _countryLookUp.FirstOrDefault(
                    c => line.IndexOf(c, StringComparison.InvariantCultureIgnoreCase) > -1);
            if (country == null) return addressFound;

            //Assume address is in one line and ending with country name
            //Working backward to the beginning of the line to get the address
            resume.Location = line.Substring(0, line.IndexOf(country) + country.Length);

            addressFound = true;

            return addressFound;
        }

        private bool ExtractAge(Resume resume, bool ageFound, string line)
        {
            if (ageFound) return ageFound;

            if (AgeRegex.Match(line).Success)
            {
                resume.Age = line;
                ageFound = true;
            }
            return ageFound;
        }
        private bool ExtractCurrentAddress(Resume resume, bool addressFound, string line)
        {
            if (addressFound) return addressFound;

            List<string> list = new List<string>() { "现居住", "现居住于", "居住于", "现住址" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                var result = line.Trim().Split('|');
                if (result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        if (list.Any(x => item.IndexOf(x) >= 0))
                        {
                            resume.Location = item;
                            continue;
                        }
                        if (item.Contains("岁"))
                        {
                            resume.Age = item;
                            continue;
                        }
                    }
                }
                addressFound = true;
            }
            return addressFound;
        }

        //ExtractComment
        private bool ExtractComment(Resume resume, bool commentFound, string line, Section section)
        {
            if (commentFound) return commentFound;

            List<string> list = new List<string>() { "自我评价", "自我介绍", "介绍", "评价", "简介" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                int a = section.Content.IndexOf(line);
                resume.SummaryDescription = section.Content[a + 1];
                commentFound = true;
            }
            return commentFound;
        }
        //GraduateSchool
        private bool ExtractGraduateSchool(Resume resume, bool GraduateSchool, string line, Section section)
        {
            if (GraduateSchool) return GraduateSchool;

            List<string> list = new List<string>() { "学校", "毕业院校" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                int a = section.Content.IndexOf(line);
                resume.GraduateSchool = section.Content[a + 1];
                GraduateSchool = true;
            }
            return GraduateSchool;
        }
        //Major
        private bool ExtractMajor(Resume resume, bool Major, string line, Section section)
        {
            if (Major) return Major;

            List<string> list = new List<string>() { "专业" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                int a = section.Content.IndexOf(line);
                resume.Major = section.Content[a + 1];
                Major = true;
            }
            return Major;
        }
        //Edu
        private bool ExtractEdu(Resume resume, bool Edu, string line, Section section)
        {
            if (Edu) return Edu;

            List<string> list = new List<string>() { "学历学位" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                int a = section.Content.IndexOf(line);
                resume.Edu = section.Content[a + 1];
                Edu = true;
            }
            return Edu;
        }
        //MaritalStatus
        private bool ExtractMaritalStatus(Resume resume, bool MaritalStatus, string line, Section section)
        {
            if (MaritalStatus) return MaritalStatus;

            List<string> list = new List<string>() { "婚姻状况" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                int a = section.Content.IndexOf(line);
                resume.MaritalStatus = section.Content[a + 1];
                MaritalStatus = true;
            }
            else
            {
                var result = line.Trim().Split('|');
                if (result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        if (item == "已婚" || item == "未婚" ||item=="保密")
                        {
                            resume.MaritalStatus = item;
                            continue;
                        }
                    }
                }
            }
            return MaritalStatus;
        }
        //Profession
        private bool ExtractProfession(Resume resume, bool Profession, string line, Section section)
        {
            if (Profession) return Profession;

            List<string> list = new List<string>() { "行业", "期望行业" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                int a = section.Content.IndexOf(line);
                resume.Profession = section.Content[a + 1];
                Profession = true;
            }
            return Profession;
        }
        //DutyTime
        private bool ExtractDutyTime(Resume resume, bool DutyTime, string line, Section section)
        {
            if (DutyTime) return DutyTime;

            List<string> list = new List<string>() { "到岗时间" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                resume.DutyTime = line;
                DutyTime = true;
            }
            return DutyTime;
        }

        //JobNature
        private bool ExtractJobNature(Resume resume, bool JobNature, string line, Section section)
        {
            if (JobNature) return JobNature;

            List<string> list = new List<string>() { "工作类型", "工作性质" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                int a = section.Content.IndexOf(line);
                resume.JobNature = section.Content[a + 1];
                JobNature = true;
            }
            return JobNature;
        }
        //JobStatus
        private bool ExtractJobStatus(Resume resume, bool JobStatus, string line, Section section)
        {
            if (JobStatus) return JobStatus;

            List<string> list = new List<string>() { "求职状态", "目前状况" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                if (line.Substring(line.Length - 1, 1).Equals("："))
                {
                    int a = section.Content.IndexOf(line);
                    var lineStr = section.Content[a + 1];
                    if (lineStr.Contains("在职"))
                    {
                        resume.JobStatus = "在职";
                        JobStatus = true;
                    }
                    if (lineStr.Contains("离职"))
                    {
                        resume.JobStatus = "离职";
                        JobStatus = true;
                    }
                    //resume.JobStatus = lineStr;
                    JobStatus = true;
                }
            }
            else
            {
                if (line.Contains("在职"))
                {
                    resume.JobStatus = "在职";
                    JobStatus = true;
                }
                if (line.Contains("离职"))
                {
                    resume.JobStatus = "离职";
                    JobStatus = true;
                }
            }
            return JobStatus;
        }
        //Curposition
        private bool ExtractCurposition(Resume resume, bool Curposition, string line, Section section)
        {
            if (Curposition) return Curposition;

            List<string> list = new List<string>() { "职能职位", "期望职业" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                int a = section.Content.IndexOf(line);
                resume.Curposition = section.Content[a + 1];
                Curposition = true;
            }
            return Curposition;
        }
        //ExpectedSalary
        private bool ExtractExpectedSalary(Resume resume, bool ExpectedSalary, string line, Section section)
        {
            if (ExpectedSalary) return ExpectedSalary;

            List<string> list = new List<string>() { "期望薪资", "期望月薪" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                int a = section.Content.IndexOf(line);
                resume.ExpectedSalary = section.Content[a + 1];
                ExpectedSalary = true;
            }
            return ExpectedSalary;
        }
        //ExtractComment
        private bool ExtractTargetWorkPlace(Resume resume, bool TargetWorkPlace, string line, Section section)
        {
            if (TargetWorkPlace) return TargetWorkPlace;

            List<string> list = new List<string>() { "工作地区", "工作地点", "地点" };

            if (list.Any(x => line.IndexOf(x) >= 0))
            {
                int a = section.Content.IndexOf(line);
                resume.TargetWorkPlace = section.Content[a + 1];
                TargetWorkPlace = true;
            }
            return TargetWorkPlace;
        }


        private void ExtractSocialProfiles(Resume resume, string line)
        {
            var socialProfileMatches = SocialProfileRegex.Matches(line);
            foreach (Match socialProfileMatch in socialProfileMatches)
            {
                resume.SocialProfiles.Add(socialProfileMatch.Value);
            }
        }

        private bool ExtractPhone(Resume resume, bool phoneFound, string line)
        {
            if (phoneFound) return phoneFound;

            //var lineStr = line.Replace("/", "");

            var phoneMatch = PhoneRegex.Match(line);
            if (!phoneMatch.Success) return phoneFound;

            resume.PhoneNumbers = phoneMatch.Value;

            phoneFound = true;

            return phoneFound;
        }

        private bool ExtractGender(Resume resume, bool genderFound, string line)
        {
            if (genderFound) return genderFound;

            if (line.IndexOf("男", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                resume.Gender = "男";

                genderFound = true;
            }

            if (line.IndexOf("女", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                resume.Gender = "女";

                genderFound = true;
            }

            return genderFound;
        }

        private bool ExtractFirstAndLastName(Resume resume, bool firstNameFound, string line)
        {
            if (firstNameFound) return firstNameFound;

            char[] words=null;
            if (line.Contains(" "))
            {
                var str=line.Split(' ');
                words = str[0].ToArray();
            }
            else
            {
                words = line.ToArray();
            }

            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i].ToString();
                if (!firstNameFound && _firstNameLookUp.Contains(word))
                {
                    resume.FirstName = word;

                    //Consider the rest of the line as part of last name
                    resume.LastName = string.Join(" ", words.Skip(i + 1));

                    firstNameFound = true;
                }
            }

            return firstNameFound;
        }

        private bool ExtractName(Resume resume, bool firstNameFound, string line)
        {
            var words = SplitByWhiteSpaceRegex.Split(line);

            if (firstNameFound) return firstNameFound;

            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i].Trim();
                if (!firstNameFound && _firstNameLookUp.Contains(word))
                {
                    resume.FirstName = word;

                    //Consider the rest of the line as part of last name
                    resume.LastName = string.Join(" ", words.Skip(i + 1));

                    firstNameFound = true;
                }
            }

            return firstNameFound;
        }


        private bool ExtractEmail(Resume resume, bool emailFound, string line)
        {
            if (emailFound) return emailFound;

            var emailMatch = EmailRegex.Match(line);
            if (!emailMatch.Success) return emailFound;

            resume.EmailAddress = emailMatch.Value;

            emailFound = true;

            return emailFound;
        }
    }
}
