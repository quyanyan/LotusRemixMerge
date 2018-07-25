using System.Collections.Generic;
using Model.Models;

namespace Model
{
    public class Section
    {
        public SectionType Type { get; set; }
        public List<string> Content { get; set; }
        //区分中文0/英文1
        public int StringType { get; set; }

        public Section()
        {
            Content = new List<string>();
        }
    }
}
