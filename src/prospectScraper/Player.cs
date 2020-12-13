using System;
using System.IO;
using System.Text.RegularExpressions;

namespace prospectScraper
{
    public class Player
    {
        public string FirstName { get; set; }
        public string FullName => string.Concat(FullName, " ", LastName);
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public static int ConvertHeightToInches(string height, string playerName)
        {
            // Height might look something like "\"6'1\"\"\"" - convert to inches to look less awful.
            string regexHeight = Regex.Match(height, @"\d'\d+").Value;
            string[] feetAndInches = regexHeight.Split("'");

            bool parseFeet = int.TryParse(feetAndInches[0], out int feet);
            int inches = 0;
            bool parseInches = false;
            if (feetAndInches.Length > 1 && feetAndInches[1] != null)
            {
                parseInches = int.TryParse(feetAndInches[1], out inches);
            }

            if (parseFeet && parseInches)
            {
                int heightInInches = (feet * 12) + inches;
                return heightInInches;
            }
            
            File.AppendAllText($"logs{Path.DirectorySeparatorChar}Mismatches.log", $"Player {playerName} height of {height} not converted properly, entering 0 instead" + Environment.NewLine);
                return 0;
        }
    }
}
