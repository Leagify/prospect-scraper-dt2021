using System;

namespace prospectScraper
{

    public class ProspectRanking
    {
        public int Rank { get; set; }
        public string Change { get; set; }
        public string PlayerName { get; set; }
        public string School { get; set; }
        public string Position1 { get; set; }
        public string Height { get; set; }
        public int Weight { get; set; }
        public string CollegeClass { get; set; }
        public DateTime RankingDate { get; set; }
        public string RankingDateString { get; set; }
        public string DraftStatus { get; set; }

        public ProspectRanking(DateTime date, int rank, string chg, string name, string school, string pos1, string height = "0", int weight = 0, string year = "", string draftstatus = "")
        {
            this.RankingDate = date;
            this.Rank = rank;
            this.Change = chg;
            this.PlayerName = name;
            this.School = school;
            this.Position1 = pos1;
            this.Height = height;
            this.Weight = weight;
            this.CollegeClass = year;
            this.RankingDateString = date.ToString("yyyy-MM-dd");
            this.DraftStatus = draftstatus;
        }
    }
}