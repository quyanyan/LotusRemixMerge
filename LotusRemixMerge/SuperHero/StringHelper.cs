using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SuperHero
{
    public static class StringHelper
    {
        private static List<string> _keyWords= new List<string>()
        {
            "教育经历", "自我评价","工作经验","工作描述","项目描述","责任描述","教育经历",
            @"\d{4}[-|年|\./](\d{1,2}).(\d{4}[-|年|\./](\d{1,2})|至今)"
        };
        public static List<string> MergerLine(this List<string> list)
        {
            List<string> temp = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                //Match key-words. If matched,append to last element.
                var a = (from key in _keyWords
                         where key.Any(list[i].Contains)
                         select key).Count();

                bool isDate = Regex.Match(list[i], @"\d{4}[-|年|\./](\d{1,2}).(\d{4}[-|年|\./](\d{1,2})|至今)").Success;
                if (isDate)
                {
                    temp.Add(list[i]);
                    continue;
                }
                if (a == 0 || !isDate)
                {
                    temp.Add(list[i]);
                }
                else
                {
                    list[i] = list[i - 1] + list[i];
                }
            }

            return temp;
        }


    }
}
