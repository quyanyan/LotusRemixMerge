using System.Collections.Generic;
using System.Text.RegularExpressions;
using Model;
using Model.Models;
using System.Linq;
using System.Threading;

namespace Processor
{
    public class SectionExtractor
    {
        private const int SectionTitleNumberOfWordsLimit = 4;
        private static readonly Regex SplitByWhiteSpaceRegex = new Regex(@"\s+", RegexOptions.Compiled);
        private readonly SectionMatchingService _sectionMatchingService;

        public SectionExtractor()
        {
            _sectionMatchingService = new SectionMatchingService();
        }

        public IList<Section> ExtractFrom(IList<string> rawInput)
        {
            var sections = new List<Section>();

            var i = 0;

            //TODO:解析中英文

            var personalSection = new Section
            {
                Type = SectionType.Personal
            };

            //TODO:区分中文英，立Flag 中文0，英文1
            var personalChina = 0;
            var personalEnglish = 0;

            while (i < rawInput.Count - 1 && FindSectionType(rawInput[i].ToLower()) == SectionType.Unknown)
            {
                if (!string.IsNullOrWhiteSpace(rawInput[i].Replace("\r\a", "")))
                {
                    //TODO:区分中文英，立Flag 中文0，英文1
                    var rawInputStr = rawInput[i].Replace("/", "");
                    if (!string.IsNullOrEmpty(rawInputStr))
                    {
                        personalSection.Content.Add(rawInputStr);
                        if (GetStrType(rawInputStr.Replace("：", "").Replace("、", "").Replace(",", "").Replace("(", "").Replace(")", "")) == 0)
                        { personalChina++; }
                        else
                        { personalEnglish++; }
                    }
                }
                i++;
            }
            personalSection.StringType = (personalChina > personalEnglish ? 0 : 1);

            sections.Add(personalSection);

            while (i < rawInput.Count)
            {
                var input = rawInput[i].ToLower();
                #region 区分中英文，后期作为混合简历可再优化 
                //区分中英文，后期作为混合简历可再优化
                //var inputStr = GetStrType(input.Replace("：", "").Replace("、", "").Replace(",", "").Replace("(", "").Replace(")", ""));
                #endregion
                var sectionType = FindSectionType(input);
                if (sectionType != SectionType.Unknown)
                {
                    //Starting of a new section
                    var section = new Section
                    {
                        Type = sectionType
                    };
                    int personalChinaItem = 0, personalEnglishItem = 0;
                    //var a=FindSectionType(rawInput[i + 1].ToLower());
                    while (i < rawInput.Count - 1 && (FindSectionType(rawInput[i + 1].ToLower()) == SectionType.Unknown))
                    {
                        i++;
                        string ins = rawInput[i].Replace("\r\a", "");
                        //区分中英文，后期作为混合简历可再优化
                        //if (GetStrType(ins.Replace("：", "").Replace("、", "").Replace(",", "").Replace("(", "").Replace(")", ""))!= inputStr)
                        //{ break; }
                        if (!string.IsNullOrWhiteSpace(ins))
                        {
                            section.Content.Add(ins);
                        }
                    }
                    //区分中英文，后期作为混合简历可再优化
                    if (GetStrType(input.Replace("：", "").Replace("、", "").Replace(",", "").Replace("(", "").Replace(")", "")) == 0)
                    { personalChinaItem++; }
                    else
                    { personalEnglishItem++; }
                    section.StringType = (personalChinaItem > personalEnglishItem ? 0 : 1);

                    if (section.Content.Count > 0)
                    {
                        sections.Add(section);
                    }
                }

                #region 区分中英文，后期作为混合简历可再优化 
                ////区分中英文，后期作为混合简历可再优化
                //if (GetStrType(input.Replace("：", "").Replace("、", "").Replace(",", "").Replace("(", "").Replace(")", "")) == 0)
                //{ personalChina++; }
                //else
                //{ personalEnglish++; }
                #endregion
                i++;
            }
            //推理排版格式
            int flaglast = default(int);
            Section s = sections.Where(x => x.StringType == flaglast).LastOrDefault();
            if (s != null)
            {
                int typeId = 0;
                var personalSectionSplit = new Section { Type = SectionType.Personal };
                List<int> removId = new List<int>();
                int sameTypeCount = 0;
                for (int m = s.Content.Count - 1; m >= 0; m--)
                {
                    var item = s.Content[m];
                    typeId = GetStrType(item.Replace("：", "").Replace("、", "").Replace(",", "").Replace("(", "").Replace(")", ""));
                    //获取第一条信息type
                    if (typeId != s.StringType)
                    {
                        sameTypeCount++;
                        personalSectionSplit.Content.Add(item);
                        personalSectionSplit.StringType = typeId;
                        removId.Add(m);
                        // s.Content.Remove(item);
                    }
                }
                if (sameTypeCount != 0)
                {
                    //不平分情况
                    if (s.Content.Count / sameTypeCount != 2 || (s.Content.Count / sameTypeCount == 2 && s.Content.Count % sameTypeCount != 0))
                    {
                        foreach (var reId in removId)
                        {
                            s.Content.Remove(s.Content[reId]);
                        }
                        personalSectionSplit.Content.Reverse();
                        sections.Add(personalSectionSplit);
                    }
                }
            }
            return sections;
        }


        /// <summary>
        /// 区分中0/英文1
        /// </summary>
        public int GetStrType(string str)
        {
            Regex regAllEnglish = new Regex(@"^[a-zA-Z0-9]+$");
            Regex regAllChina = new Regex(@"^[\u4e00-\u9fa5]+$");
            Regex regContainChina = new Regex(@"[\u4e00-\u9fa5\a-zA-Z\S]");
            int flag = 0;
            if (regAllEnglish.IsMatch(str))
            {
                flag = 1;
            }
            else if (regAllChina.IsMatch(str))
            {
                //中文
                flag = 0;
            }
            else if (regContainChina.IsMatch(str))
            {
                //混合中英文，需判断中文或者英文出现的概率
                if (GetChineseLeng(str) >= GetEnglishLeng(str))
                {
                    flag = 0;
                }
                else
                {
                    flag = 1;
                }
            }
            return flag;
        }

        public int GetChineseLeng(string StrContert)
        {
            if (string.IsNullOrEmpty(StrContert))
                return 0;
            //此处进行的是一些转义符的替换,以免照成统计错误.
            StrContert = StrContert.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            //替换掉内容中的数字,英文,空格.方便进行字数统计.
            string str = Regex.Matches(StrContert, @"[A-Za-z0-9][A-Za-z0-9'\-.]*").ToString().Replace("　", "").Replace(" ", "");
            int iChinese = getByteLengthByChinese(Regex.Matches(StrContert, @"[A-Za-z0-9][A-Za-z0-9'\-.]*").ToString().Replace("　", "").Replace(" ", ""));
            int a = Regex.Matches(StrContert, @"[\u4e00-\u9fa5]").Count;
            return Regex.Matches(StrContert, @"[\u4e00-\u9fa5]").Count;//中文中的字数统计需要加上数字统计. Regex.Matches(StrContert, @"[\u4e00-\u9fa5]").Count+iChinese
        }

        /// <summary>
        /// 返回字节数 
        ///Author:Lynn
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int getByteLengthByChinese(string s)
        {
            int l = 0;
            char[] q = s.ToCharArray();
            for (int i = 0; i < q.Length; i++)
            {
                if ((int)q[i] >= 0x4E00 && (int)q[i] <= 0x9fbb) // 0x9FA5
                {
                    l += 1;
                }
            }
            return l;
        }
        public static int GetEnglishLeng(string StrContert)
        {
            string strRepValues = "\n,\r,nbsp;,lt;brgt;";
            if (string.IsNullOrEmpty(StrContert))
                return 0;
            StrContert = StrContert.Replace("&", " ");
            string[] arrSplit = strRepValues.Split(',');
            for (int i = 0; i < arrSplit.Length; i++)
            {
                StrContert = StrContert.Replace(arrSplit[i], " ");
            }
            return Regex.Matches(StrContert, @"[A-Za-z][A-Za-z'\-.]*").Count;
        }


        //TODO:修改方法包含通过英文分词分割后的title
        private SectionType FindSectionType(string input)
        {
            string ins = input.Replace("\r\a", "");

            //if (input.Contains("自我评价"))
            //{
            //    Thread.Sleep(1);
            //}

            if (!string.IsNullOrEmpty(ins))
            {
                var elements = SplitByWhiteSpaceRegex.Split(ins);

                var a = elements.Length < SectionTitleNumberOfWordsLimit ? _sectionMatchingService.FindSectionTypeMatching(input) : SectionType.Unknown;
                return a;
            }
            return SectionType.Courses;
        }


        private bool IsChinese(string text)
        {
            return Regex.IsMatch(text, @"[\u4e00-\u9fbb]+{1}");
        }
    }
}
