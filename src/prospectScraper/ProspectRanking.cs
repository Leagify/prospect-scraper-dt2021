using System;

namespace prospectScraper
{
    public class ProspectRanking
    {
        public DateTime Date { get; set; }
        public DateTime RankingDate { get; set; }
        public int Rank { get; set; }
        public int Weight { get; set; }
        public string Change { get; set; }
        public string CollegeClass { get; set; }
        public string DraftStatus { get; set; }
        public string Height { get; set; }
        public string PlayerName { get; set; }
        public string Position1 { get; set; }
        public string RankingDateString => Date.ToString("yyyy-MM-dd");
        public string School { get; set; }
    }
}