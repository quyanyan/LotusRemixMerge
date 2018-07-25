using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Processor.Helpers
{
    public static class CommonHelper
    {
        public static Dictionary<string, string> StrLength(string str)
        {
            Dictionary<string, string> lstCount = new Dictionary<string, string>();
            int lenC = 0;
            string strC = "";
            int lenE = 0;
            string strE = "";
            for (int i = 0; i < str.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(str.Substring(i, 1));
                if (byte_len.Length > 1)
                {
                    lenC += 2;
                    strC += str.Substring(i, 1);
                }
                else
                {
                    lenE += 1;
                    strE += str.Substring(i, 1);
                }
            }
            lstCount.Add("lenC_" + lenC, strC);
            lstCount.Add("lenE_" + lenE, strE);
            return lstCount;
        }
        public static string CleanInvalidCharsForText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            else
            {
                StringBuilder checkedStringBuilder = new StringBuilder();
                Char[] chars = input.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    int charValue = Convert.ToInt32(chars[i]);

                    if ((charValue >= 0x00 && charValue <= 0x08) || (charValue >= 0x0b && charValue <= 0x0c) || (charValue >= 0x0e && charValue <= 0x1f))
                        continue;
                    else
                        checkedStringBuilder.Append(chars[i]);
                }

                return checkedStringBuilder.ToString();
            }
        }        
    }
}
