using System.Collections.Generic;

namespace Model
{
    public class Resume
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumbers { get; set; }
        public string Languages { get; set; }
        public string SummaryDescription { get; set; }
        public string Age { get; set; }

        //"毕业学校"
        public string GraduateSchool { get; set; }
        //"专业"
        public string Major { get; set; }
        //"学历"
        public string Edu { get; set; }
        //"婚姻状态"
        public string MaritalStatus { get; set; }
        //"行业"
        public string Profession { get; set; }

        //"到岗时间"
        public string DutyTime { get; set; }
        //"工作性质（全职、兼职）"
        public string JobNature { get; set; }
        //"求职状态 （在职、找工作）"
        public string JobStatus { get; set; }
        //"公司职位
        public string Curposition { get; set; }
       
        //"期望月薪（存数字还是字符串？）"
        public string ExpectedSalary { get; set; }
        //"目标工作地点（可以多个）"
        public string TargetWorkPlace { get; set; }


        public List<string> Skills { get; set; }
        public string Location { get; set; }
        public List<Position> Positions { get; set; }
        public List<Project> Projects { get; set; }
        public List<string> SocialProfiles { get; set; }
        public List<Education> Educations { get; set; }
        public List<string> Courses { get; set; }
        public List<string> Awards { get; set; }

        public Resume()
        {
            Skills = new List<string>();
            Positions = new List<Position>();
            Projects = new List<Project>();
            SocialProfiles = new List<string>();
            Educations = new List<Education>();
            Courses = new List<string>();
            Awards = new List<string>();
        }
    }
}
