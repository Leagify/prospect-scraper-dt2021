using CsvHelper.Configuration;

namespace prospectScraper.Maps
{
    public sealed class ProspectRankingMap : ClassMap<ProspectRanking>
    {
        public ProspectRankingMap()
        {
            Map(m => m.Rank).Index(0).Name("Rank");
            Map(m => m.Change).Index(1).Name("Change");
            Map(m => m.PlayerName).Index(2).Name("Player");
            Map(m => m.School).Index(3).Name("School");
            Map(m => m.Position1).Index(4).Name("Position");
            Map(m => m.Height).Index(5).Name("Height");
            Map(m => m.Weight).Index(6).Name("Weight");
            Map(m => m.CollegeClass).Index(7).Name("CollegeClass");
            Map(m => m.RankingDateString).Index(8).Name("Date");
            Map(m => m.DraftStatus).Index(9).Name("DraftStatus");

        }
    }
}