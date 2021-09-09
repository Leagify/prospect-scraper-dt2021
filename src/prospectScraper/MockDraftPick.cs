using CsvHelper;
using prospectScraper.Maps;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace prospectScraper
{
    public class MockDraftPick
    {
        public int Round { get; set; }
        public string TeamCity { get; set; }
        public string PickNumber { get; set; }
        public string PlayerName { get; set; }
        public string School { get; set; }
        public int Pick { get; set; }
        public string Position { get; set; }
        public string ReachValue { get; set; }
        public int LeagifyPoints => ConvertPickToPoints(Pick.ToString(), this.Round);
        public string PickDate { get; set; }
        public string State { get; set; }

        public static int ConvertPickToRound(string pick)
        {
            bool canParse = int.TryParse(pick, out int intPick);

            if (!canParse)
                return 0;

            return intPick switch
            {
                /*
                Pick numbers without comp picks:
                Picks 1 - 32 : Round 1
                    Picks 33 - 64: Round 2
                    Picks 65 - 96: Round 3
                    Picks 97 - 128: Round 4
                    Picks 129 - 159: Round 5
                    Picks 160 - 191: Round 6
                    Picks 192 - 223: Round 7
                    Pick numbers with comp picks:
                Round 1 = picks 1 - 32
                    Round 2 = picks 33 - 64
                    Round 3 = picks 65 - 103
                    Round 4 = picks 104 - 146
                    Round 5 = picks 147 - 179
                    Round 6 = picks 180 - 214
                    Round 7 = picks 215 - 255
                */
                >= 1 and <= 32 => 1,
                >= 33 and <= 64 => 2,
                >= 65 and <= 103 => 3,
                >= 104 and <= 146 => 4,
                >= 147 and <= 179 => 5,
                >= 180 and <= 214 => 6,
                >= 215 and <= 255 => 7,
                _ => 0
            };
        }

        public static int ConvertPickToPoints(string pick, int round)
        {
            bool canParse = int.TryParse(pick, out int intPick);
            if (!canParse) return 0;
            return intPick switch
            {
                /*
                Top Pick: 40 Points
                Picks 2 - 10: 35 Points
                Picks 11 - 20: 30 Points
                Picks 21 - 32: 25 Points
                Picks 33 - 48: 20 Points
                Picks 49 - 64: 15 Points
                Round 3: 10 Points
                Round 4: 8 Points
                Round 5: 7 Points
                Round 6: 6 Points
                Round 7: 5 Points
                */
                1 => 40,
                >= 2 and <= 10 => 35,
                >= 11 and <= 20 => 30,
                >= 21 and <= 32 => 25,
                >= 33 and <= 48 => 20,
                >= 49 and <= 64 => 15,
                _ => round switch
                {
                    3 => 10,
                    4 => 8,
                    5 => 7,
                    6 => 6,
                    7 => 5,
                    _ => 0
                }
            };
        }

        public static string GetState(string school)
        {
            List<School> schoolsAndConferences;
            using (var reader = new StreamReader($"info{Path.DirectorySeparatorChar}SchoolStatesAndConferences.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                csv.Configuration.RegisterClassMap<SchoolCsvMap>();
                schoolsAndConferences = csv.GetRecords<School>().ToList();
            }
            var stateResults = from s in schoolsAndConferences
                              where s.SchoolName == school
                              select s.State;
#nullable enable
            string? stateResult = stateResults.FirstOrDefault();
#nullable disable
            string sr = string.Empty;

            if (stateResult != null)
            {
                sr = stateResult;
            }
            else
            {
                Console.WriteLine("Error matching school!");
            }

            return sr.Length > 0 ? sr : string.Empty;
        }
    }
}