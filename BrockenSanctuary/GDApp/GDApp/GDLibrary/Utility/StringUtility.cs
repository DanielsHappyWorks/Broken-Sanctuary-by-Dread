using System.Text.RegularExpressions;

namespace GDLibrary
{
    public class StringUtility
    {
        //parse a file name from a path + name string
        public static string ParseNameFromPath(string path)
        { //"Assets/Textures/sky"
            return Regex.Match(path, @"[^\\/]*$").Value;
        }
    }
}
