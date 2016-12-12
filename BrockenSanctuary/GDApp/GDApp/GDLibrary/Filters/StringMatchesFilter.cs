using System.Text.RegularExpressions;
namespace GDLibrary
{
    public class StringMatchesFilter : IFilter<string>
    {
        private string regex;
        public StringMatchesFilter(string regex)
        {
            this.regex = regex;
        }

        public bool Matches(string s)
        {
            return Regex.IsMatch(s, this.regex);
        }
    }
}
