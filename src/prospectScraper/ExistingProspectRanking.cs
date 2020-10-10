namespace prospectScraper
{
    public class ExistingProspectRanking
    {
        public string rank;
        public string change;
        public string playerName;
        public string school;
        public string position1;
        public string height;
        public string weight;
        public string collegeClass;
        public string rankingDateString;
        public string draftStatus;

        public ExistingProspectRanking()
        {
        }

        public ExistingProspectRanking(string rank, string chg, string name, string school, string pos1, string height, string weight, string year, string date, string status)
        {
            this.rank = rank;
            this.change = chg;
            this.playerName = name;
            this.school = school;
            this.position1 = pos1;
            this.height = height;
            this.weight = weight;
            this.collegeClass = year;
            this.rankingDateString = date;
            this.draftStatus = status;
        }
    }
}