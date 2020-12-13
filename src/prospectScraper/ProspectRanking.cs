using System;

namespace prospectScraper
{

    public class ProspectRanking
    {
        public int Rank { get; set; }
        public string Change { get; set; }
        public DateTime Date { get; set; }
        public string PlayerName { get; set; }
        public string School { get; set; }
        public string Position1 { get; set; }
        public string Height { get; set; }
        public int Weight { get; set; }
        public string CollegeClass { get; set; }
        public DateTime RankingDate { get; set; }

        public string RankingDateString => this.Date.ToString("yyyy-MM-dd") ?? this.RankingDate.ToString("yyyy-MM-dd");
        public string DraftStatus { get; set; }
    }
}