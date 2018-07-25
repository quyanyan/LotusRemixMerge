using System.Collections.Generic;
using Model;
using Model.Models;
using Processor.Helpers;
using System.Linq;
using System.Reflection;

namespace Processor.Parsers
{
    public class ProjectsParserCN : IParser
    {
        public void Parse(Section section, Resume resume)
        {
            resume.Projects = new List<Project>();

            bool isFindAValue = false;
            string propertyName = string.Empty;
            System.Type type;//获取类型
            PropertyInfo propertyInfo;
            string flagValue = string.Empty;
            Project currentProject = new Project();
            int flagStartFirstLine = 0;//判断经验第一行是否为时间标识，默认为0:不是
            for (int i = 0; i < section.Content.Count; i++)
            {
                var line = CommonHelper.CleanInvalidCharsForText(section.Content[i]);
                if (propertyName != string.Empty)
                {
                    type = currentProject.GetType();//获取类型
                    propertyInfo = type.GetProperty(propertyName);
                    flagValue = propertyInfo.GetValue(currentProject, null).ToString();
                }               
                var companyName = string.Empty;
                //将时间取值提前,用于后期判断
                var startAndEndDate = DateHelper.ParseStartAndEndDateCN(line);
                if (GetCompanyNameString(line, out companyName, section))//获取公司名字
                {
                    if (flagValue != string.Empty && propertyName == "CampanyName")
                    {
                        resume.Projects.Add(currentProject);//加集合
                        //添加判断，如果项目经验第一行是以时间为起始行是实例新的项目经验
                        if (!string.IsNullOrEmpty(currentProject.StartDate) && flagStartFirstLine == 1)
                        {
                            currentProject = new Project();
                        }
                    }

                    if (!isFindAValue)
                    {
                        propertyName = "CampanyName";
                    }
                    if (line.Substring(line.Length - 1, 1).Equals("："))
                    {
                        currentProject.CampanyName = section.Content[i + 1];
                    }
                    else
                    {
                        currentProject.CampanyName = companyName;
                    }
                    i++;
                    isFindAValue = true;
                    continue;
                }
                else
                {
                    if (flagValue != string.Empty && propertyName == "Summary")
                    {
                        //添加判断，如果项目经验第一行是以时间为起始行是实例新的项目经验
                        if (!string.IsNullOrEmpty(currentProject.StartDate) && flagStartFirstLine == 1)
                        {
                            resume.Projects.Add(currentProject);//加集合
                            currentProject = new Project();
                        }
                    }

                    if (!isFindAValue)
                    {
                        propertyName = "Summary";
                    }

                    isFindAValue = true;
                    if (startAndEndDate == null)
                        currentProject.Summary.Add(line);
                }

                if (startAndEndDate != null)
                {
                    if (flagValue != string.Empty && propertyName == "StartDate")
                    {
                        //添加判断，如果项目经验第一行是以时间为起始行是实例新的项目经验
                        if (flagStartFirstLine == 1)
                        {
                            resume.Projects.Add(currentProject);//加集合
                            currentProject = new Project();
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
                    currentProject.StartDate = startAndEndDate.Start;
                    currentProject.EndDate = startAndEndDate.End;
                    //添加判断，如果项目经验第一行是时间，下一项默认为项目名称
                    if ((i == 0 || flagStartFirstLine == 1) && !string.IsNullOrEmpty(currentProject.StartDate))
                    {
                        flagStartFirstLine = 1;//标识为1:是
                        currentProject.Title = section.Content[i + 1];
                        i++;
                    }
                    else
                    {
                        int sumCount = currentProject.Summary.Count;//找到时间节点，统计之前summary之前的项数量
                        currentProject.Title = section.Content[i - sumCount];//默认第一项为项目名称
                        currentProject.Summary.Remove(section.Content[i - sumCount]);
                    }
                    isFindAValue = true;
                    continue;
                }

                if (i == (section.Content.Count - 1))
                {
                    resume.Projects.Add(currentProject);//加集合
                }
            }
        }
        private bool GetCompanyNameString(string line, out string companyName, Section section)
        {

            List<string> list = new List<string>() { "集团", "有限公司", "责任公司", "技术公司", "科技公司", "所属公司" };
            companyName = "";
            bool result = false;
            if (list.Any(x => line.IndexOf(x) >= 0) && line.Length < 15)
            {
                companyName = line;
                result = true;
            }
            return result;
        }
    }
    //int i = 0;
    //while (i < section.Content.Count)
    //{
    //    var line = section.Content[i];
    //    var startAndEndDate = DateHelper.ParseStartAndEndDate(line);
    //    if (startAndEndDate == null)
    //    {
    //        if (currentProject != null)
    //        {
    //            currentProject.Summary.Add(line);
    //        }
    //        else if (line.IndexOf(':') > -1)
    //        {
    //            var elements = line.Split(':');
    //            var project = new Project
    //            {
    //                Title = elements[0]
    //            };
    //            project.Summary.Add(elements[1]);

    //            resume.Projects.Add(project);
    //        }
    //    }
    //    else
    //    {
    //        currentProject = new Project
    //        {
    //            StartDate = startAndEndDate.Start,
    //            EndDate = startAndEndDate.End,
    //            Title = line.Substring(0, line.IndexOf(startAndEndDate.Start)).Trim()
    //        };

    //        resume.Projects.Add(currentProject);
    //    }

    //    i++;
    //}

}
