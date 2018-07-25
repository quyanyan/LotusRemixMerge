using System.Text.RegularExpressions;
using Model;

namespace Processor.Helpers
{
    public class DateHelper
    {
        private const string ShortMonth = "Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec";
        private const string FullMonth = "January|February|March|April|May|June|July|August|September|October|November|December";
        private static readonly Regex StartAndEndDateRegex =
            new Regex(
                string.Format(
                    @"(?<Start>({0}|{1}|\d{{1,2}})[/\s-–](20)?\d{{2}})[/\s-–—]+(?<End>({0}|{1}|\d{{1,2}})[/\s-–](20)?\d{{2}}|Current|Now|Present)",
                    ShortMonth, FullMonth), RegexOptions.Compiled);

        //private static readonly Regex StartAndEndDateRegexCN = new Regex(@"\d{4}[-|年|\./][\d{1}|\d{2}].(\d{4}[-|年|\./][\d{1}|\d{2}]|至今)");
        private static readonly Regex StartAndEndDateRegexCN1 = new Regex(@"\d{6}.\d{6}|至今");
        private static readonly Regex StartAndEndDateRegexCN = new Regex(@"\d{4}[-|年|\./](\d{1,2}).(\d{4}[-|年|\./](\d{1,2})|至今)");//\d{4}[-|年|\./]([0]?[1-9]|[1][0-2]).(\d{4}[-|年|\./]([0]?[1-9]|[1][0-2])|至今)

        public static Period ParseStartAndEndDate(string input)
        {
            var match = StartAndEndDateRegex.Match(input);
            if (match.Success)
            {
                var startDate = match.Groups["Start"].Value;
                var endDate = match.Groups["End"].Value;

                return new Period(startDate, endDate);
            }

            return null;
        }

        public static Period ParseStartAndEndDateCN(string input)
        {

            var match = StartAndEndDateRegexCN.Match(input.Replace(" ", "").Replace("--","-"));
            if (match.Success)
            {
                var a = match.Value.Split('-');
                var startDate = a[0];
                var endDate = a[1];

                return new Period(startDate, endDate);
            }

            return null;
        }
    }
}
