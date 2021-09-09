﻿using CsvHelper;
using HtmlAgilityPack;
using prospectScraper.DTOs;
using prospectScraper.Maps;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace prospectScraper
{
    public class ProspectScraper
    {
        private readonly HtmlWeb _webGet = new()
        {
            UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0"
        };

        public static async Task RunTheBigBoardsAsync(bool parseDate = true)
        {
            await File.WriteAllTextAsync($"logs{Path.DirectorySeparatorChar}Status.log", "");
            await File.WriteAllTextAsync($"logs{Path.DirectorySeparatorChar}Prospects.log", "");

            Console.WriteLine("Getting data...");

            var webGet = new HtmlWeb
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0"
            };
            Console.WriteLine("Testing webGet.Load...");
            Console.WriteLine("Getting data...");
            var document1 = webGet.Load("https://www.drafttek.com/2021-NFL-Draft-Big-Board/Top-NFL-Draft-Prospects-2021-Page-1.asp");
            var document2 = webGet.Load("https://www.drafttek.com/2021-NFL-Draft-Big-Board/Top-NFL-Draft-Prospects-2021-Page-2.asp");
            var document3 = webGet.Load("https://www.drafttek.com/2021-NFL-Draft-Big-Board/Top-NFL-Draft-Prospects-2021-Page-3.asp");
            var document4 = webGet.Load("https://www.drafttek.com/2021-NFL-Draft-Big-Board/Top-NFL-Draft-Prospects-2021-Page-4.asp");
            var document5 = webGet.Load("https://www.drafttek.com/2021-NFL-Draft-Big-Board/Top-NFL-Draft-Prospects-2021-Page-5.asp");

            Console.WriteLine("Parsing data...");

            //Get ranking date
            string dateOfRanks = document1
                .DocumentNode
                .SelectSingleNode("//*[@id='HeadlineInfo1']")
                .InnerText
                .Replace(" EST", "")
                .Trim();
            
            var parsedDate = ChangeDateStringToDateTime(dateOfRanks, parseDate);
            
            //Change date to proper date. The original format should be like this:
            //" May 21, 2019 2:00 AM EST"
            string dateInNiceFormat = parsedDate.ToString("yyyy-MM-dd");

            var list1 = GetProspects(document1, parsedDate, 1);
            var list2 = GetProspects(document2, parsedDate, 2);
            var list3 = GetProspects(document3, parsedDate, 3);
            var list4 = GetProspects(document4, parsedDate, 4);
            var list5 = GetProspects(document5, parsedDate, 5);

            //This is the file name we are going to write.
            string csvFileName = $"ranks{Path.DirectorySeparatorChar}{dateInNiceFormat}-ranks.csv";

            Console.WriteLine("Creating csv...");

            //Write projects to csv with date.
            await using (var writer = new StreamWriter(csvFileName))
            await using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
            {
                csv.Configuration.RegisterClassMap<ProspectRankingMap>();
                await csv.WriteRecordsAsync(list1);
                await csv.WriteRecordsAsync(list2);
                await csv.WriteRecordsAsync(list3);
                
                if (list4.Any())
                {
                    await csv.WriteRecordsAsync(list4);
                }
                if (list5.Any())
                {
                    await csv.WriteRecordsAsync(list5);
                }
            }

            CheckForMismatches(csvFileName);
            CreateCombinedCsv();
            CheckForMismatches($"ranks{Path.DirectorySeparatorChar}combinedRanks2021.csv");
            await CreateCombinedCsvWithExtras();

            Console.WriteLine("Big Board Completed.");
        }

        public async Task RunTheMockDraft(bool parseDate)
        {
            string draftDate = string.Empty;

            var listOfDraftPicks = new List<MockDraftPick>();
            for (int i = 1; i < 7; i++)
            {
                var sb = new StringBuilder($"https://www.drafttek.com/2021-NFL-Mock-Draft/2021-NFL-Mock-Draft-Round-");
                sb.Append($"{i}.asp");
                var doc = _webGet.Load(sb.ToString());
                if (i == 1)
                {
                    // Need to get date of mock draft eventually.
                    var hn = doc.DocumentNode;
                    var hi1 = hn.SelectSingleNode("//*[@id='HeadlineInfo1']");
                    var mockDraftDate = ChangeDateStringToDateTime(hi1.InnerText.Replace(" EST", "").Trim(), parseDate);
                    draftDate = mockDraftDate.ToString("yyyy-MM-dd");
                }
                listOfDraftPicks.AddRange(GetMockDraft(doc, draftDate));
            }

            Console.WriteLine("Creating csv...");
            await using (var writer = new StreamWriter($"mocks{Path.DirectorySeparatorChar}{draftDate}-mock.csv"))
            await using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
            {
                csv.Configuration.RegisterClassMap<MockDraftPickMap>();
                await csv.WriteRecordsAsync(listOfDraftPicks);
            }

            Console.WriteLine("Checking for mock draft mismatches...");
            CheckForMockDraftMismatches(listOfDraftPicks, "All drafts.");
            CheckForMockDraftMismatches($"mocks{Path.DirectorySeparatorChar}{draftDate}-mock.csv");
            Console.WriteLine("Behold, the draft! Mock Draft Completed.");
        }

        public static DateTime ChangeDateStringToDateTime(string scrapedDate, bool parseDate)
        {
            //Change date to proper date. The original format should be like this:
            //" May 21, 2019 2:00 AM EST"
            bool parseWorks = DateTime.TryParse(scrapedDate, out var parsedDate);

            if (parseDate && parseWorks)
            {
                return parsedDate;
            }
            return DateTime.Now;
        }

        public static IEnumerable<MockDraftPick> GetMockDraft(HtmlDocument doc, string pickDate)
        {
            var mdpList = new List<MockDraftPick>();
            // This is still messy from debugging the different values.  It should be optimized.
            var dn = doc.DocumentNode;
            var dns = dn.SelectNodes("/html/body/div/div/div/table");
            foreach (var node in dns)
            {
                bool hasTheStyle = node
                    .Attributes
                    .FirstOrDefault()
                    .Value
                    .Contains("background-image: linear-gradient", StringComparison.OrdinalIgnoreCase);
                if (hasTheStyle)
                {
                    var tr = node.SelectSingleNode("tr");
                    var mockDraftPick = CreateMockDraftEntry(tr, pickDate);
                    mdpList.Add(mockDraftPick);
                }
            }
            return mdpList;
        }

        public static MockDraftPick CreateMockDraftEntry(HtmlNode tableRow, string pickDate)
        {
            var childNodes = tableRow.ChildNodes;
            var node1 = childNodes[1].InnerText; //pick number?
            string pickNumber = node1.Replace("\r", "")
                                    .Replace("\n", "")
                                    .Replace("\t", "")
                                    .Replace(" ", "");
            var node3 = childNodes[3]; //team (and team image)?
            var teamCity = node3.ChildNodes[0].InnerText
                                    .Replace("\r", "")
                                    .Replace("\n", "")
                                    .Replace("\t", "")
                                    .TrimEnd();
            var node5 = childNodes[5]; //Has Child Nodes - Player, School, Position, Reach/Value
            string playerName = node5.ChildNodes[1].InnerText
                                    .Replace("\r", "")
                                    .Replace("\n", "")
                                    .Replace("\t", "")
                                    .TrimEnd();
            string playerSchoolBeforeChecking = node5.ChildNodes[3].InnerText
                                    .Replace("\r", "")
                                    .Replace("\n", "")
                                    .Replace("\t", "")
                                    .TrimEnd(); // this may have a space afterwards.
            string playerSchool = School.CheckSchool(playerSchoolBeforeChecking);
            string playerPosition = node5.ChildNodes[5].InnerText
                                    .Replace("\r", "")
                                    .Replace("\n", "")
                                    .Replace("\t", "")
                                    .Replace(" ", "");
            string reachValue = node5.ChildNodes[9].InnerText
                                    .Replace("\r", "")
                                    .Replace("\n", "")
                                    .Replace("\t", "")
                                    .Replace(" ", "");

            var mdp = new MockDraftPick()
            {
                Pick = int.Parse(pickNumber),
                PickNumber = pickNumber,
                TeamCity = teamCity,
                PlayerName = playerName,
                School = playerSchool,
                Position = playerPosition,
                ReachValue = reachValue,
                PickDate = pickDate
            };
            File.AppendAllText($"logs{Path.DirectorySeparatorChar}Prospects.log", "Mock Draft Round: " + mdp.Round + Environment.NewLine);
            File.AppendAllText($"logs{Path.DirectorySeparatorChar}Prospects.log", "Pick Number: " + mdp.PickNumber + Environment.NewLine);
            File.AppendAllText($"logs{Path.DirectorySeparatorChar}Prospects.log", "Player: " + mdp.PlayerName + Environment.NewLine);
            return mdp;
        }

        public static string FormatDraftDate(string headlineInfo)
        {
            string hi2 = headlineInfo.Replace(" EST", "").Trim();
            // TODO: Change date to proper date. The original format should be like this:
            //" May 21, 2019 2:00 AM EST"
            bool parseWorks = DateTime.TryParse(hi2, out var parsedDate);
            string dateInNiceFormat;
            if (parseWorks)
            {
                dateInNiceFormat = parsedDate.ToString("yyyy-MM-dd");
            }
            else
            {
                dateInNiceFormat = DateTime.Now.ToString("yyyy-MM-dd");
            }

            Console.WriteLine("Mock Draft - Date parsed: " + parsedDate + " - File name will be: " + dateInNiceFormat + "-mock.csv");
            return dateInNiceFormat;
        }

        private static void CreateCombinedCsv()
        {
            //Combine ranks from CSV files to create a master CSV.
            var filePaths = Directory.GetFiles($"ranks{Path.DirectorySeparatorChar}", "20??-??-??-ranks.csv").ToList<String>();
            //The results are probably already sorted, but I don't trust that, so I'm going to sort manually.
            filePaths.Sort();

            // Specify wildcard search to match CSV files that will be combined
            using var fileDest = new StreamWriter($"ranks{Path.DirectorySeparatorChar}combinedRanks2021.csv", false);
            for (int i = 0; i < filePaths.Count; i++)
            {
                string file = filePaths[i];

                string[] lines = File.ReadAllLines(file);

                if (i > 0)
                {
                    lines = lines.Skip(1).ToArray(); // Skip header row for all but first file
                }

                foreach (string line in lines)
                {
                    fileDest.WriteLine(line);
                }
            }
        }

        private static IEnumerable<School> SchoolsAndConferencesFromCsv()
        {
            using var reader = new StreamReader($"info{Path.DirectorySeparatorChar}SchoolStatesAndConferences.csv");
            using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
            csv.Configuration.RegisterClassMap<SchoolCsvMap>();
            return csv.GetRecords<School>().ToList();
        }

        private static IEnumerable<Region> StatesAndRegionsFromCsv()
        {
            using var reader = new StreamReader($"info{Path.DirectorySeparatorChar}StatesToRegions.csv");
            using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
            csv.Configuration.RegisterClassMap<RegionCsvMap>();
            return csv.GetRecords<Region>().ToList();
        }

        private static IEnumerable<PositionType> PositionsAndTypesFromCsv()
        {
            using var reader = new StreamReader($"info{Path.DirectorySeparatorChar}PositionInfo.csv");
            using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
            csv.Configuration.RegisterClassMap<PositionTypeCsvMap>();
            return csv.GetRecords<PositionType>().ToList();
        }

        private static async Task CreateCombinedCsvWithExtras()
        {
            await File.AppendAllTextAsync($"logs{Path.DirectorySeparatorChar}Status.log", "Creating the big CSV....." + Environment.NewLine);

            var schoolsAndConferences = SchoolsAndConferencesFromCsv();
            var statesAndRegions = StatesAndRegionsFromCsv();
            var positionsAndTypes = PositionsAndTypesFromCsv();

            // Let's assign these ranks point values.
            List<PointProjection> ranksToProjectedPoints;
            using (var reader = new StreamReader($"info{Path.DirectorySeparatorChar}RanksToProjectedPoints.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                csv.Configuration.RegisterClassMap<PointProjectionCsvMap>();
                ranksToProjectedPoints = csv.GetRecords<PointProjection>().ToList();
            }

            //Combine ranks from CSV files to create a master CSV.
            var filePaths = Directory.GetFiles($"ranks{Path.DirectorySeparatorChar}", "20??-??-??-ranks.csv").ToList<String>();
            //The results are probably already sorted, but I don't trust that, so I'm going to sort manually.
            filePaths.Sort();
            string destinationFile = $"ranks{Path.DirectorySeparatorChar}joinedRanks2021.csv";

            // Specify wildcard search to match CSV files that will be combined
            var fileDest = new StreamWriter(destinationFile, false);

            for (int i = 0; i < filePaths.Count; i++)
            {
                string file = filePaths[i];

                string[] lines = await File.ReadAllLinesAsync(file);

                if (i > 0)
                {
                    lines = lines.Skip(1).ToArray(); // Skip header row for all but first file
                }

                foreach (string line in lines)
                {
                    await fileDest.WriteLineAsync(line);
                }
            }

            fileDest.Close();

            // Get ranks from the newly created CSV file.
            List<ExistingProspectRanking> prospectRanks;
            using (var reader = new StreamReader($"ranks{Path.DirectorySeparatorChar}joinedRanks2021.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                csv.Configuration.RegisterClassMap<ExistingProspectRankingCsvMap>();
                prospectRanks = csv.GetRecords<ExistingProspectRanking>().ToList();
            }

            // Use linq to join the stuff back together, then write it out again.
            var combinedHistoricalRanks = from r in prospectRanks
                                          join school in schoolsAndConferences
                                            on r.School equals school.SchoolName
                                          join region in statesAndRegions
                                            on school.State equals region.State
                                          join positions in positionsAndTypes
                                            on r.Position1 equals positions.PositionName
                                          join rank in ranksToProjectedPoints
                                            on r.Rank equals rank.Rank
                                          select new
                                          {
                                              r.Rank,
                                              r.Change,
                                              Name = r.PlayerName,
                                              Position = r.Position1,
                                              College = r.School,
                                              school.Conference,
                                              school.State,
                                              Region = region.RegionCode,
                                              r.Height,
                                              r.Weight,
                                              r.CollegeClass,
                                              positions.PositionGroup,
                                              positions.PositionAspect,
                                              ProspectStatus = r.DraftStatus,
                                              Date = r.RankingDateString,
                                              Points = rank.ProjectedPoints
                                          };

            //Write everything back to CSV, only better!
            await using (var writer = new StreamWriter($"ranks{Path.DirectorySeparatorChar}joinedRanks2021.csv"))
            await using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
            {
                await csv.WriteRecordsAsync(combinedHistoricalRanks);
            }

            await File.AppendAllTextAsync($"logs{Path.DirectorySeparatorChar}Status.log", "Creating the big CSV completed." + Environment.NewLine);
        }

        private static void CheckForMismatches(string csvFileName)
        {
            File.AppendAllText($"logs{Path.DirectorySeparatorChar}Status.log", "Checking for mismatches in " + csvFileName + "....." + Environment.NewLine);

            // Read in data from a different project.
            List<School> schoolsAndConferences;
            using (var reader = new StreamReader($"info{Path.DirectorySeparatorChar}SchoolStatesAndConferences.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                csv.Configuration.RegisterClassMap<SchoolCsvMap>();
                schoolsAndConferences = csv.GetRecords<School>().ToList();
            }

            List<ProspectRankSimple> ranks;
            using (var reader = new StreamReader(csvFileName))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                csv.Configuration.RegisterClassMap<ProspectRankSimpleCsvMap>();
                ranks = csv.GetRecords<ProspectRankSimple>().ToList();
            }

            var schoolMismatches = Mismatches(ranks, schoolsAndConferences).ToList();

            bool noMismatches = true;

            if (schoolMismatches.Any())
            {
                File.WriteAllText($"logs{Path.DirectorySeparatorChar}Mismatches.log", "");
            }

            foreach (var s in schoolMismatches)
            {
                noMismatches = false;
                File.AppendAllText($"logs{Path.DirectorySeparatorChar}Mismatches.log", $"{s.Rank}, {s.Name}, {s.College}" + Environment.NewLine);
            }

            if (noMismatches)
            {
                File.AppendAllText($"logs{Path.DirectorySeparatorChar}Status.log", "No mismatches in " + csvFileName + "....." + Environment.NewLine);
            }
            else
            {
                File.AppendAllText($"logs{Path.DirectorySeparatorChar}Status.log", schoolMismatches.Count() + " mismatches in " + csvFileName + ".....Check Mismatches.log." + Environment.NewLine);
            }
        }

        public static List<ProspectRanking> GetProspects(HtmlDocument document, DateTime dateOfRanks, int pageNumber)
        {
            var prospectList = new List<ProspectRanking>();

            if (document.DocumentNode != null)
            {
                var tbl = document.DocumentNode.SelectNodes("/html[1]/body[1]/div[1]/div[3]/div[1]/table[1]");

                if (tbl == null)
                {
                    File.AppendAllText($"logs{Path.DirectorySeparatorChar}Status.log", $"No prospects on page {pageNumber}" + Environment.NewLine);
                    return prospectList;
                }

                // Create variables to store prospect rankings.
                int rank = 0;
                string change = "";
                string playerName = "";
                string school = "";
                string position1 = "";
                string height = "";
                int weight = 0;
                string collegeClass = "";

                foreach (var table in tbl)
                {
                    foreach (var row in table.SelectNodes("tr"))
                    {

                        foreach (var cell in row.SelectNodes("th|td"))
                        {

                            string Xpath = cell.XPath;
                            int locationOfColumnNumber = cell.XPath.Length - 2;
                            char dataIndicator = Xpath[locationOfColumnNumber];
                            bool isRank = (dataIndicator == '1');
                            switch (dataIndicator)
                            {
                                case '1':
                                    // td[1]= Rank
                                    if (Int32.TryParse(cell.InnerText, out int rankNumber))
                                        rank = rankNumber;
                                    File.AppendAllText($"logs{Path.DirectorySeparatorChar}Prospects.log", "Big Board Rank: " + cell.InnerText + Environment.NewLine);
                                    break;
                                case '2':
                                    // td[2]= Change
                                    change = cell.InnerText;
                                    change = change.Replace("&nbsp;", "");
                                    break;
                                case '3':
                                    // td[3]= Player
                                    playerName = cell.InnerText;
                                    File.AppendAllText($"logs{Path.DirectorySeparatorChar}Prospects.log", "Player: " + cell.InnerText + Environment.NewLine);
                                    break;
                                case '4':
                                    // td[4]= School
                                    school = School.CheckSchool(cell.InnerText);
                                    break;
                                case '5':
                                    // td[5]= Pos1
                                    position1 = cell.InnerText;
                                    break;
                                case '6':
                                    // td[6]= Ht
                                    height = cell.InnerText;
                                    break;
                                case '7':
                                    // td[7]= Weight
                                    if (Int32.TryParse(cell.InnerText, out int weightNumber))
                                        weight = weightNumber;
                                    break;
                                case '8':
                                    // College Class- used to be Pos2 (which was often blank)
                                    collegeClass = cell.InnerText;
                                    break;
                                case '9':
                                    // td[9]= Link to Bio (not used)
                                    continue;
                            }
                        }
                        // Handle draft eligibility and declarations (done via row color)
                        string draftStatus = "";
                        if (row.Attributes.Contains("style") && row.Attributes["style"].Value.Contains("background-color"))
                        {
                            string rowStyle = row.Attributes["style"].Value;
                            string backgroundColor = Regex.Match(rowStyle, @"background-color: \w*").Value.Substring(18);
                            draftStatus = backgroundColor switch
                            {
                                "white" => "Eligible",
                                "lightblue" => "Underclassman",
                                "palegoldenrod" => "Declared",
                                _ => "",
                            };
                            File.AppendAllText($"logs{Path.DirectorySeparatorChar}Prospects.log", "Draft Status: " + draftStatus + Environment.NewLine);
                        }
                        // The header is in the table, so I need to ignore it here.
                        if (change != "CNG")
                        {
                            prospectList.Add(new ProspectRanking()
                            {
                                Date = dateOfRanks,
                                Rank = rank,
                                Change = change,
                                PlayerName = playerName,
                                School = school,
                                Position1 = position1,
                                Height = height,
                                Weight = weight,
                                CollegeClass = collegeClass,
                                DraftStatus = draftStatus
                            });
                        }
                    }
                }
                File.AppendAllText($"logs{Path.DirectorySeparatorChar}Status.log", $"Prospect count on page {pageNumber}: {prospectList.Count}" + Environment.NewLine);
            }
            return prospectList;
        }

        private static void CheckForMockDraftMismatches(List<MockDraftPick> listOfPicks, string description)
        {
            File.AppendAllText($"logs{Path.DirectorySeparatorChar}Status.log", $"Checking for mismatches in {description}...{Environment.NewLine}");

            var schoolMismatches = Mismatches(listOfPicks, SchoolsAndConferences()).ToList();

            if (!schoolMismatches.Any())
            {
                File.AppendAllText($"logs{Path.DirectorySeparatorChar}Status.log", "No mismatches in " + description + "....." + Environment.NewLine);
            }
            else
            {
                File.WriteAllText($"logs{Path.DirectorySeparatorChar}Mismatches.log", "");
                foreach (var s in schoolMismatches)
                {
                    File.AppendAllText($"logs{Path.DirectorySeparatorChar}Mismatches.log", $"Mock draft mismatch: {s.Rank}, {s.Name}, {s.College}" + Environment.NewLine);
                }
                File.AppendAllText($"logs{Path.DirectorySeparatorChar}Status.log", schoolMismatches.Count + " mismatches in list of picks from " + description + ".....Check Mismatches.log." + Environment.NewLine);
            }
        }

        private static List<School> SchoolsAndConferences()
        {
            using var reader = new StreamReader($"info{Path.DirectorySeparatorChar}SchoolStatesAndConferences.csv");
            using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
            csv.Configuration.RegisterClassMap<SchoolCsvMap>();
            return csv.GetRecords<School>().ToList();
        }

        private static void CheckForMockDraftMismatches(string csvFileName)
        {
            File.AppendAllText($"logs{Path.DirectorySeparatorChar}Status.log", "Checking for mismatches in " + csvFileName + "....." + Environment.NewLine);

            var ranks = new List<MockDraftPick>();
            using (var reader = new StreamReader(csvFileName))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                csv.Configuration.RegisterClassMap<MockDraftPickCsvMap>();
                ranks = csv.GetRecords<MockDraftPick>().ToList();
            }

            var schoolMismatches = new List<SchoolMismatchDTO>();
            using (var reader = new StreamReader($"info{Path.DirectorySeparatorChar}SchoolStatesAndConferences.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                csv.Configuration.RegisterClassMap<SchoolCsvMap>();
                schoolMismatches = Mismatches(ranks, csv.GetRecords<School>().ToList()).ToList();
            }

            if (!schoolMismatches.Any())
            {
                foreach (var s in schoolMismatches)
                {
                    File.AppendAllText($"logs{Path.DirectorySeparatorChar}Mismatches.log", $"{s.Rank}, {s.Name}, {s.College}" + Environment.NewLine);
                }
                File.AppendAllText($"logs{Path.DirectorySeparatorChar}Status.log", $"No mismatches in {csvFileName}...{Environment.NewLine}");
            }
            else
            {
                File.AppendAllText($"logs{Path.DirectorySeparatorChar}Status.log", $"{schoolMismatches.Count} mismatches in {csvFileName}...Check Mismatches.log.{Environment.NewLine}");
            }
        }

        private static IEnumerable<SchoolMismatchDTO> Mismatches(IEnumerable<ProspectRankSimple> draftPicks, IEnumerable<School> schools)
        {
            return from r in draftPicks
                   join school in schools
                       on r.School equals school.SchoolName into mm
                   from school in mm.DefaultIfEmpty()
                   where school is null
                   select new SchoolMismatchDTO()
                   {
                       Rank = r.Rank.ToString(),
                       Name = r.PlayerName,
                       College = r.School
                   };
        }

        private static IEnumerable<SchoolMismatchDTO> Mismatches(IEnumerable<MockDraftPick> draftPicks, IEnumerable<School> schools)
        {
            return from r in draftPicks
                   join school in schools
                       on r.School equals school.SchoolName into mm
                   from school in mm.DefaultIfEmpty()
                   where school is null
                   select new SchoolMismatchDTO()
                   {
                       Rank = r.PickNumber,
                       Name = r.PlayerName,
                       College = r.School
                   };
        }
    }
}
